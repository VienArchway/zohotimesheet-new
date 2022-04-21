using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;
using System.Net;
using System.Web;

namespace api.Infrastructure.Clients
{
    public class TaskItemClient : ZohoServiceClient, ITaskItemClient
    {
        public TaskItemClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(
            DateTime? startDateFrom,
            DateTime? startDateTo,
            IEnumerable<int> sprintTypeIds,
            IEnumerable<string> completedOn,
            int statusId,
            IEnumerable<string> assignees)
        {
            var result = new List<TaskItem>();
            var index = 1;
            var hasNext = true;
            var range = 100;

            while (hasNext)
            {
                var filter = new JObject();
                var view = 1;
                if (assignees != null && assignees.Any())
                {
                    filter.Add("assignee", JArray.FromObject(assignees));
                    view = 0;
                }

                if (startDateFrom != null && startDateTo != null)
                {
                    filter.Add("startdate_fromdate", startDateFrom.Value.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                    filter.Add("startdate_todate", startDateTo.Value.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                    filter.Add("startdate", JArray.FromObject(new string[] { "custom", "withoutdue" }));
                }

                if (sprintTypeIds != null && sprintTypeIds.Any())
                {
                    filter.Add("sprinttype", JArray.FromObject(sprintTypeIds));
                }

                if (completedOn != null && completedOn.Any())
                {
                    filter.Add("completedon", JArray.FromObject(completedOn));
                }

                var url = $"team/{teamId}/globalview/?action=TaskItemdetails&view={view}&viewoption={statusId}&index={index}&range={range}&needsubTaskItem=true";

                var filterEncode = HttpUtility.UrlEncode(JsonConvert.SerializeObject(filter));
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

                var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                var hasData = srcJObj.GetValue("hasData").ToObject<bool>();

                if (hasData)
                {
                    hasNext = srcJObj.GetValue("next").ToObject<bool>();
                    var TaskItemProperties = srcJObj.GetValue("TaskItem_prop");
                    var TaskItems = srcJObj.GetValue("TaskItemJObj");
                    var projItemTypeProperties = srcJObj.GetValue("projItemType_prop");
                    var projItemTypes = srcJObj.GetValue("projItemTypeJObj");
                    var projStatusProperties = srcJObj.SelectToken("projStatus.status_Prop");
                    var projStatuses = srcJObj.SelectToken("projStatus").Children().Last().Last();
                    var userTaskItems = srcJObj.GetValue("userDisplayName");
                    var users = ConvertUserDisplay(userTaskItems);

                    var resultTaskItems = ConvertJsonResponseToClass<TaskItem>(TaskItemProperties, TaskItems);
                    var resultProjItemTypes = ConvertJsonResponseToClass<ProjectItemType>(projItemTypeProperties, projItemTypes);
                    var resultprojStatuses = ConvertJsonResponseToClass<ProjectStatus>(projStatusProperties, projStatuses);

                    foreach (var resultTaskItem in resultTaskItems)
                    {
                        resultTaskItem.Users = users;
                        resultTaskItem.ProjTaskItemName = resultProjItemTypes.FirstOrDefault(TaskItem => TaskItem.ProjItemTypeId.Equals(resultTaskItem.ProjItemTypeId)).ItemTypeName;

                        var status = resultprojStatuses.FirstOrDefault(TaskItem => TaskItem.StatusId.Equals(resultTaskItem.StatusId));
                        resultTaskItem.StatusName = status != null ? status.Name : null;

                        if (resultTaskItem.IsParent)
                        {
                            var urlSubTaskItems = $"team/{teamId}/projects/{resultTaskItem.ProjId}/sprints/{resultTaskItem.SprintId}/TaskItem/{resultTaskItem.TaskItemId}/subTaskItem/?action=level";
                            var resSubTaskItems = await client.GetAsync(urlSubTaskItems).ConfigureAwait(false);

                            if (resSubTaskItems.StatusCode == HttpStatusCode.OK)
                            {
                                var resContentSubTaskItems = await resSubTaskItems.Content.ReadAsStringAsync().ConfigureAwait(false);
                                var srcJObjSubTaskItems = JsonConvert.DeserializeObject<JObject>(resContentSubTaskItems);
                                var subIds = srcJObjSubTaskItems.GetValue("TaskItemIds").ToObject<List<string>>();
                                resultTaskItem.SubTaskItemIds = subIds;
                            }
                        }
                    }

                    result.AddRange(resultTaskItems);
                    index += range;
                }
                else
                {
                    hasNext = false;
                }
            }

            return result;
        }

        public async Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(string projectId, IEnumerable<string> TaskItemIds, IEnumerable<string> TaskItemNos)
        {
            var result = new List<TaskItem>();
            var filter = new JObject();
            var url = $"team/{teamId}/projects/{projectId}/TaskItem/?action=multipledetails";
            if (TaskItemIds != null && TaskItemIds.Any())
            {
                var TaskItemidarr = HttpUtility.UrlEncode(JsonConvert.SerializeObject(TaskItemIds));
                url += $"&TaskItemidarr={TaskItemidarr}";
            }

            if (TaskItemNos != null && TaskItemNos.Any())
            {
                var TaskItemnoarr = HttpUtility.UrlEncode(JsonConvert.SerializeObject(TaskItemNos));
                url += $"&TaskItemidarr={TaskItemnoarr}";
            }

            var response = await client.GetAsync(url).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(responseContent);
            }

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);

            var TaskItemProperties = srcJObj.GetValue("TaskItem_prop");
            var TaskItems = srcJObj.GetValue("TaskItemJObj");
            var userTaskItems = srcJObj.GetValue("userDisplayName");
            var users = ConvertUserDisplay(userTaskItems);

            var resultTaskItems = ConvertJsonResponseToClass<TaskItem>(TaskItemProperties, TaskItems);

            foreach (var resultTaskItem in resultTaskItems)
            {
                resultTaskItem.Users = users;

                if (resultTaskItem.IsParent)
                {
                    var urlSubTaskItems = $"team/{teamId}/projects/{resultTaskItem.ProjId}/sprints/{resultTaskItem.SprintId}/TaskItem/{resultTaskItem.TaskItemId}/subTaskItem/?action=level";
                    var resSubTaskItems = await client.GetAsync(urlSubTaskItems).ConfigureAwait(false);

                    if (resSubTaskItems.StatusCode == HttpStatusCode.OK)
                    {
                        var resContentSubTaskItems = await resSubTaskItems.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var srcJObjSubTaskItems = JsonConvert.DeserializeObject<JObject>(resContentSubTaskItems);
                        var subIds = srcJObjSubTaskItems.GetValue("TaskItemIds").ToObject<List<string>>();
                        resultTaskItem.SubTaskItemIds = subIds;
                    }
                }

                result.AddRange(resultTaskItems);
            }

            result = result.OrderBy(TaskItem => TaskItem.TaskItemId).ToList();

            return result;
        }

        public async Task UpdateStatusAsync(UpdateTaskItemStatusParameter parameter)
        {
            var url = $"/zsapi/team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/TaskItem/{parameter.TaskItemId}/";
            var content = SetAndEncodeParameter(parameter);

            var response = await client.PostAsync(url, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var status = srcJObj.GetValue("status")?.ToObject<string>();

            if (status == null || !status.Equals("success"))
            {
                throw new InvalidOperationException(responseContent);
            }
        }

        public async Task<TaskItem> CreateAsync(TaskItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/TaskItem/";
            var formContent = SetAndEncodeParameter(parameter);

            var response = await SendTaskItem(url, formContent).ConfigureAwait(false);
            var result = GetTaskItemFromResponse(response);
            return result.FirstOrDefault();
        }

        public async Task<TaskItem> CreateSubTaskItemAsync(TaskItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/TaskItem/{parameter.TaskItemId}/subTaskItem/";
            var formContent = SetAndEncodeParameter(parameter);

            var response = await SendTaskItem(url, formContent).ConfigureAwait(false);
            var result = GetTaskItemFromResponse(response);
            return result.FirstOrDefault();
        }

        public async Task UpdateAsync(TaskItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/TaskItem/{parameter.TaskItemId}/";
            var formContent = SetAndEncodeParameter(parameter);

            await SendTaskItem(url, formContent).ConfigureAwait(false);
        }

        public async Task DeleteAsync(DeleteTaskItemParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/TaskItem/{parameter.TaskItemId}/";
            var formContent = SetAndEncodeParameter(parameter);

            await SendTaskItem(url, formContent).ConfigureAwait(false);
        }

        private async Task<JObject> SendTaskItem(string url, FormUrlEncodedContent formContent)
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

        private IEnumerable<TaskItem> GetTaskItemFromResponse(JObject srcJObj, Project proj = null)
        {
            var TaskItemProperties = srcJObj.GetValue("TaskItem_prop");
            var TaskItems = srcJObj.GetValue("TaskItemJObj");
            var userTaskItems = srcJObj.GetValue("userDisplayName");
            var users = ConvertUserDisplay(userTaskItems);

            var resultTaskItems = ConvertJsonResponseToClass<TaskItem>(TaskItemProperties, TaskItems);
            return resultTaskItems;
        }
    }
}