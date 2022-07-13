using System.Net.Http.Headers;
using api.Application.Interfaces;
using api.Application.Resolver;
using api.Infrastructure.Interfaces;
using api.Models;
using Microsoft.PowerBI.Api;
using Newtonsoft.Json;
using Slack.Webhooks;

namespace api.Application
{
    public class AdlsService : IAdlsService
    {
        private readonly ILogWorkService logWorkService;

        private readonly IAdlsClient adlsClient;

        private readonly ISlackWebHookClient slackClient;

        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly IBackgroundTaskQueue queueTaskService;

        private readonly String logWorkFilePath;

        private readonly String logWorkFilePathBackUp;

        private readonly String powerBIGroupId;

        private readonly String dataSetId;

        private readonly PowerBIClient powerBIClient;

        public AdlsService(
            ILogWorkService logWorkService,
            IAdlsClient adlsClient,
            IConfiguration configuration,
            IBackgroundTaskQueue queueTaskService,
            IServiceScopeFactory serviceScopeFactory,
            ISlackWebHookClient slackClient,
            PowerBIClient powerBIClient)
        {            
            this.logWorkService = logWorkService;
            this.adlsClient = adlsClient;
            this.slackClient = slackClient;
            this.serviceScopeFactory = serviceScopeFactory;
            this.queueTaskService = queueTaskService;
            this.powerBIClient = powerBIClient;
            this.logWorkFilePath = configuration.GetValue<String>("Adls:LogWorkFilePath");
            this.logWorkFilePathBackUp = configuration.GetValue<String>("Adls:LogWorkFilePathBackUp");
            this.powerBIGroupId = configuration.GetValue<String>("PowerBI:PowerBIGroupId");
            this.dataSetId = configuration.GetValue<String>("PowerBI:DataSetId");
        }

        public async Task<IEnumerable<LogWork>> GetFromAdlsAsync()
        {
            var isExist = adlsClient.CheckExist(logWorkFilePath);
            if (!isExist)
            {
                throw new FileNotFoundException();
            }

            var content = await adlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);
            return !String.IsNullOrEmpty(content) ? JsonConvert.DeserializeObject<LogWorkFileContent>(content).Logs : Enumerable.Empty<LogWork>();
        }

        public async Task DeleteFromAdlsAsync(String[] AdlsIds)
        {
            var isExist = adlsClient.CheckExist(logWorkFilePath);

            if (!isExist)
            {
                await adlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
            }

            if (queueTaskService.Status)
            {
                throw new InvalidOperationException("Can not upload data because data file is using, please try later!");
            }

            var currentAdlsContent = await adlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);
            var currentAdlsData = JsonConvert.DeserializeObject<LogWorkFileContent>(currentAdlsContent);
            var newData = currentAdlsData == null ? Enumerable.Empty<LogWork>() : currentAdlsData.Logs.Where(x => !AdlsIds.Contains(x.LogTimeId)).ToList();

            var content = JsonConvert.SerializeObject(new LogWorkFileContent() { Logs = newData });
            await adlsClient.UploadAsync(content, logWorkFilePath).ConfigureAwait(false);
            await RefreshPowerBIDataAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<LogWork>> TransferAdlsAsync(LogWorkSearchParameter parameter, String userTransfer = null)
        {
            var data = await logWorkService.SearchAsync(parameter).ConfigureAwait(false);
            var isExist = adlsClient.CheckExist(logWorkFilePath);
            var isBackUpExist = adlsClient.CheckExist(logWorkFilePathBackUp);

            if (!isExist)
            {
                await adlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
            }

            if (!isBackUpExist)
            {
                await adlsClient.CreateFileAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            }

            if (queueTaskService.Status)
            {
                throw new InvalidOperationException("Can not upload data because data file is using, please try later!");
            }

            var currentAdlsContent = await adlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);

            // to back up before process new data
            var fileInfo = await adlsClient.GetPropertiesAsync(logWorkFilePath).ConfigureAwait(false);
            var fileBackUpInfo = await adlsClient.GetPropertiesAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            if (fileInfo.ContentLength > fileBackUpInfo.ContentLength)
            {
                await adlsClient.UploadAsync(currentAdlsContent, logWorkFilePathBackUp).ConfigureAwait(false);
            }

            // remove data duplicate and upload
            var currentAdlsData = JsonConvert.DeserializeObject<LogWorkFileContent>(currentAdlsContent);
            var newData = currentAdlsData == null ? data : currentAdlsData.Logs.Concat(data).GroupBy(p => p.LogTimeId).Select(g => g.Last()).ToList();
            var content = JsonConvert.SerializeObject(new LogWorkFileContent() { Logs = newData }, new JsonSerializerSettings()
            {
                ContractResolver = new LogWorkIgnorePropertiesResolver(new[] { "projNo" })
            });

            await adlsClient.UploadAsync(content, logWorkFilePath).ConfigureAwait(false);
            var fileInfoAfterTranfer = await adlsClient.GetPropertiesAsync(logWorkFilePath).ConfigureAwait(false);

            if (fileInfo.ContentLength > fileInfoAfterTranfer.ContentLength)
            {
                #pragma warning disable S1075
                var url = "https://portal.azure.com/#@archwayprod.onmicrosoft.com/resource/subscriptions/fb2ad051-e5bd-483a-a02b-99d7d5d23539/resourceGroups/archway-actualanalysis-amimata-20190717/providers/Microsoft.DataLakeStore/accounts/analysis/overview";
                #pragma warning restore S1075
                slackClient.SendMessage(
                    "An error occurs when transfer Zoho Adls!",
                    "#D00000",
                    Emoji.ArrowsCounterclockwise,
                    "Zoho Adls Transfer Report",
                    new List<SlackField>
                    {
                        new SlackField { Value = $"An error occurs when transfer Zoho Adls, it makes the file lose data! Please check on <{url}|Azure Data Lake>" }
                    });
            }
            else
            {
                await RefreshPowerBIDataAsync().ConfigureAwait(false);
            }

            return data;
        }

        public async Task TransferAdlsAsync(String logId, String ownerId, DateTime logDate)
        {
            var param = new LogWorkSearchParameter()
            {
                    OwnerIds = new List<String>() { ownerId },
                    StartDate = logDate,
                    EndDate = logDate
            };
            var adlsItems = await logWorkService.SearchAsync(param).ConfigureAwait(false);
            var data = adlsItems.Where(x => x.LogTimeId == logId);
            var isExist = adlsClient.CheckExist(logWorkFilePath);

            if (!isExist)
            {
                await adlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
            }

            queueTaskService.QueueBackgroundWorkItem(async token =>
            {
                try
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var newAdlsClient = scopedServices.GetRequiredService<IAdlsClient>();

                        var currentAdlsContent = newAdlsClient.Download(logWorkFilePath);
                        var currentAdlsData = JsonConvert.DeserializeObject<LogWorkFileContent>(currentAdlsContent);
                        var newData = currentAdlsData == null ? data : currentAdlsData.Logs.Concat(data).GroupBy(p => p.LogTimeId).Select(g => g.Last()).ToList();
                        var content = JsonConvert.SerializeObject(new LogWorkFileContent() { Logs = newData }, new JsonSerializerSettings()
                        {
                            ContractResolver = new LogWorkIgnorePropertiesResolver(new[] { "projNo" })
                        });

                        newAdlsClient.Upload(content, logWorkFilePath);

                        await Task.Delay(TimeSpan.FromSeconds(1), token).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    foreach (var Adls in data)
                    {
                        var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        message += $"\nParameters: ";
                        message += $"\n- LogId: {Adls.LogTimeId}";
                        message += $"\n- Owner Name: {Adls.OwnerName}";
                        message += $"\n- Task Item: {Adls.ItemName}";
                        message += $"\n- Log Date: {Adls.LogDate}";
                        message += $"\n- Log Time: {Adls.LogTime}";

                        slackClient.SendMessage(
                            "An error occurs when operate Zoho Adls Webhook!",
                            "#D00000",
                            Emoji.Clock1,
                            "Zoho Adls WebHook Report",
                            new List<SlackField>
                            {
                                new SlackField
                                    {
                                        Value = $"*Detail*: {message}"
                                    }
                            });
                    }
                }
            });
        }

        public async Task RestoreFromAdlsAsync()
        {
            var isBackUpExist = adlsClient.CheckExist(logWorkFilePathBackUp);
            if (!isBackUpExist)
            {
                throw new FileNotFoundException();
            }

            var backedUpAdlsContent = await adlsClient.DownloadAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            await adlsClient.UploadAsync(backedUpAdlsContent, logWorkFilePath).ConfigureAwait(false);
        }

        public async Task RefreshPowerBIDataAsync()
        {
            try
            {
                await powerBIClient.Datasets.RefreshDatasetAsync(Guid.Parse(powerBIGroupId), dataSetId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                if (message.Contains("Operation returned an invalid status code 'BadRequest'"))
                {
                    message = "Can not refresh PowerBI dataset because the refresh execution times are limited. Please refresh manually!";
                }

                slackClient.SendMessage(
                    "An error occurs when refresh PowerBI dataset!",
                    "#ffcc00",
                    Emoji.Warning,
                    "PowerBI Dataset Refresh Warning",
                    new List<SlackField>
                    {
                        new SlackField
                            {
                                Value = $"*Detail*: {message}"
                            }
                    });
            }
        }
    }
}