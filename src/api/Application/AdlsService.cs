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

        private readonly IAdlsClient AdlsClient;

        private readonly ISlackWebHookClient slackClient;

        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly IBackgroundTaskQueue queueTaskService;

        private readonly string logWorkFilePath;

        private readonly string logWorkFilePathBackUp;

        private readonly string powerBIGroupId;

        private readonly string dataSetId;

        private readonly PowerBIClient powerBIClient;

        public AdlsService(
            ILogWorkService logWorkService,
            IAdlsClient AdlsClient,
            IConfiguration configuration,
            IBackgroundTaskQueue queueTaskService,
            IServiceScopeFactory serviceScopeFactory,
            ISlackWebHookClient slackClient,
            PowerBIClient powerBIClient)
        {            
            this.logWorkService = logWorkService;
            this.AdlsClient = AdlsClient;
            this.slackClient = slackClient;
            this.serviceScopeFactory = serviceScopeFactory;
            this.queueTaskService = queueTaskService;
            this.powerBIClient = powerBIClient;
            this.logWorkFilePath = configuration.GetValue<string>("Adls:LogWorkFilePath");
            this.logWorkFilePathBackUp = configuration.GetValue<string>("Adls:LogWorkFilePathBackUp");
            this.powerBIGroupId = configuration.GetValue<string>("PowerBI:PowerBIGroupId");
            this.dataSetId = configuration.GetValue<string>("PowerBI:DataSetId");
        }

        public async Task<IEnumerable<LogWork>> GetFromAdlsAsync()
        {
            var isExist = AdlsClient.CheckExist(logWorkFilePath);
            if (!isExist)
            {
                throw new FileNotFoundException();
            }

            var content = await AdlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);
            return !string.IsNullOrEmpty(content) ? JsonConvert.DeserializeObject<LogWorkFileContent>(content).Logs : Enumerable.Empty<LogWork>();
        }

        public async Task DeleteFromAdlsAsync(string[] AdlsIds)
        {
            var isExist = AdlsClient.CheckExist(logWorkFilePath);

            if (!isExist)
            {
                await AdlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
            }

            if (queueTaskService.Status)
            {
                throw new InvalidOperationException("Can not upload data because data file is using, please try later!");
            }

            var currentAdlsContent = await AdlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);
            var currentAdlsData = JsonConvert.DeserializeObject<LogWorkFileContent>(currentAdlsContent);
            var newData = currentAdlsData == null ? Enumerable.Empty<LogWork>() : currentAdlsData.Logs.Where(x => !AdlsIds.Contains(x.LogTimeId)).ToList();

            var content = JsonConvert.SerializeObject(new LogWorkFileContent() { Logs = newData });
            await AdlsClient.UploadAsync(content, logWorkFilePath).ConfigureAwait(false);
            await RefreshPowerBIDataAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<LogWork>> TransferAdlsAsync(LogWorkSearchParameter parameter, string userTransfer = null)
        {
            var data = await logWorkService.SearchAsync(parameter).ConfigureAwait(false);
            var isExist = AdlsClient.CheckExist(logWorkFilePath);
            var isBackUpExist = AdlsClient.CheckExist(logWorkFilePathBackUp);

            if (!isExist)
            {
                await AdlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
            }

            if (!isBackUpExist)
            {
                await AdlsClient.CreateFileAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            }

            if (queueTaskService.Status)
            {
                throw new InvalidOperationException("Can not upload data because data file is using, please try later!");
            }

            var currentAdlsContent = await AdlsClient.DownloadAsync(logWorkFilePath).ConfigureAwait(false);

            // to back up before process new data
            var fileInfo = await AdlsClient.GetPropertiesAsync(logWorkFilePath).ConfigureAwait(false);
            var fileBackUpInfo = await AdlsClient.GetPropertiesAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            if (fileInfo.ContentLength > fileBackUpInfo.ContentLength)
            {
                await AdlsClient.UploadAsync(currentAdlsContent, logWorkFilePathBackUp).ConfigureAwait(false);
            }

            // remove data duplicate and upload
            var currentAdlsData = JsonConvert.DeserializeObject<LogWorkFileContent>(currentAdlsContent);
            var newData = currentAdlsData == null ? data : currentAdlsData.Logs.Concat(data).GroupBy(p => p.LogTimeId).Select(g => g.Last()).ToList();
            var content = JsonConvert.SerializeObject(new LogWorkFileContent() { Logs = newData }, new JsonSerializerSettings()
            {
                ContractResolver = new LogWorkIgnorePropertiesResolver(new[] { "projNo" })
            });

            await AdlsClient.UploadAsync(content, logWorkFilePath).ConfigureAwait(false);
            var fileInfoAfterTranfer = await AdlsClient.GetPropertiesAsync(logWorkFilePath).ConfigureAwait(false);

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

        public async Task TransferAdlsAsync(string logId, string ownerId, DateTime logDate)
        {
            var param = new LogWorkSearchParameter()
            {
                    OwnerIds = new List<string>() { ownerId },
                    StartDate = logDate,
                    EndDate = logDate
            };
            var AdlsItems = await logWorkService.SearchAsync(param).ConfigureAwait(false);
            var data = AdlsItems.Where(x => x.LogTimeId == logId);
            var isExist = AdlsClient.CheckExist(logWorkFilePath);

            if (!isExist)
            {
                await AdlsClient.CreateFileAsync(logWorkFilePath).ConfigureAwait(false);
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
            var isBackUpExist = AdlsClient.CheckExist(logWorkFilePathBackUp);
            if (!isBackUpExist)
            {
                throw new FileNotFoundException();
            }

            var backedUpAdlsContent = await AdlsClient.DownloadAsync(logWorkFilePathBackUp).ConfigureAwait(false);
            await AdlsClient.UploadAsync(backedUpAdlsContent, logWorkFilePath).ConfigureAwait(false);
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