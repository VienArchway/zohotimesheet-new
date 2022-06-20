using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;
using System.Web;
using System.Net;

namespace api.Infrastructure.Clients
{
    public class LogWorkClient : ZohoServiceClient, ILogWorkClient
    {
        public LogWorkClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<LogWork>> SearchAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<string> projectIds,
            IEnumerable<string> sprintTypes,
            IEnumerable<string> ownerIds,
            int delayTimeoutBySeconds = 0)
        {
            var index = 1;
            var range = 100;

            var hasData = true;
            var totalResult = new List<LogWork>();

            while (hasData)
            {
                // Because Zoho limit the number requests on a minutue, we must delay requests to avoid Zoho error response.
                var delayTask = Task.Run(async () =>
                        {
                            await Task.Delay(TimeSpan.FromSeconds(delayTimeoutBySeconds)).ConfigureAwait(false);
                        });
                delayTask.Wait();

                if (delayTask.Status == TaskStatus.RanToCompletion)
                {
                    var filter = new JObject();
                    if (projectIds != null && projectIds.Any())
                    {
                        filter.Add("projectIds", JArray.FromObject(projectIds));
                    }

                    if (ownerIds != null && ownerIds.Any())
                    {
                        filter.Add("logowner", JArray.FromObject(ownerIds));
                    }

                    if (sprintTypes != null && sprintTypes.Any())
                    {
                        filter.Add("sprinttype", JArray.FromObject(sprintTypes));
                    }

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        if (endDate == null)
                        {
                            endDate = DateTime.Now.Date;
                        }

                        filter.Add("logdate", JArray.FromObject(new string[] { "custom" }));
                        filter.Add("logdate_fromdate", startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                        filter.Add("logdate_todate", endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                    }

                    var filterEncode = HttpUtility.UrlEncode(JsonConvert.SerializeObject(filter));

                    var url = $"team/{teamId}/timesheet/?action=orglogs&viewtype=0&index={index}&range={range}&logtype=0";


                    if (!string.IsNullOrEmpty(filterEncode))
                    {
                        url += $"&filter={filterEncode}";
                    }

                    var response = await client.GetAsync(url).ConfigureAwait(false);
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new InvalidOperationException(responseContent);
                    }
                    else
                    {
                        var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                        var properties = srcJObj?.GetValue("log_prop");
                        var logworkItems = srcJObj?.GetValue("logJObj");
                        var resultTaskItems = ConvertJsonResponseToClass<LogWork>(properties, logworkItems);
                        hasData = srcJObj.GetValue("hasData").ToObject<bool>();

                        if (hasData)
                        {
                            var resultItems = GetLogWorkFromResponse(srcJObj);

                            totalResult.AddRange(resultItems);
                            index += range;
                        }
                    }
                }
            }

            return totalResult;
        }

        public async Task<LogWork> CreateAsync(LogWorkSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/{parameter.ItemId}/timesheet/";
            var formContent = SetAndEncodeParameter(parameter);

            var response = await SendLogWork(url, formContent).ConfigureAwait(false);
            var result = GetLogWorkFromResponse(response);
            return result.FirstOrDefault();
        }

        public async Task UpdateAsync(LogWorkSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/item/{parameter.ItemId}/timesheet/{parameter.LogTimeId}/";
            var formContent = SetAndEncodeParameter(parameter);

            await SendLogWork(url, formContent).ConfigureAwait(false);
        }

        private async Task<JObject> SendLogWork(string url, FormUrlEncodedContent formContent)
        {
            client.DefaultRequestHeaders.Add("x-za-reqsize", new string[] { "large" });
            var response = await client.PostAsync(url, formContent).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var status = srcJObj.GetValue("status")?.ToObject<string>();

            if (status == null || !status.Equals("success"))
            {
                throw new InvalidOperationException(responseContent);
            }

            return srcJObj;
        }

        private IEnumerable<LogWork> GetLogWorkFromResponse(JObject srcJObj, Project proj = null)
        {
            var properties = srcJObj.GetValue("log_prop");
            var items = srcJObj.GetValue("logJObj");
            var userItems = srcJObj.GetValue("userDisplayName");
            var resultItems = ConvertJsonResponseToClass<LogWork>(properties, items);
            var users = ConvertUserDisplay(userItems);

            foreach (var logwork in resultItems)
            {
                logwork.LogDate = !string.IsNullOrEmpty(logwork.LogDate) ? logwork.LogDate.Split(" ")[0] : null;
                logwork.LogTime = logwork.LogTime == null ? 0 : (logwork.LogTime / 3600000);
                logwork.OwnerName = users.FirstOrDefault(x => x.UserId.Equals(logwork.Owner)).DisplayName;

                if (proj != null)
                {
                    logwork.ProjName = proj.ProjName;
                }
            }

            return resultItems;
        }

        private IEnumerable<LogWork> GetLogWorkFromResponse(JObject srcJObj)
        {
            var properties = srcJObj.GetValue("log_prop");
            var items = srcJObj.GetValue("logJObj");
            var userItems = srcJObj.GetValue("userDisplayName");
            var resultItems = ConvertJsonResponseToClass<LogWork>(properties, items);
            var users = ConvertUserDisplay(userItems);

            foreach (var logwork in resultItems)
            {
                logwork.LogDate = !string.IsNullOrEmpty(logwork.LogDate) ? logwork.LogDate.Split(" ")[0] : null;
                logwork.LogTime = logwork.LogTime == null ? 0 : (logwork.LogTime / 3600000);
                logwork.OwnerName = users.FirstOrDefault(x => x.UserId.Equals(logwork.Owner)).DisplayName;
            }

            return resultItems;
        }
    }
}