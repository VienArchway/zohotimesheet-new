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

                var url = $"team/{teamId}/globalview/?action=itemdetails&view={view}&viewoption={statusId}&index={index}&range={range}&needsubitem=true";

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
                    var itemProperties = srcJObj.GetValue("item_prop");
                    var items = srcJObj.GetValue("itemJObj");
                    var projItemTypeProperties = srcJObj.GetValue("projItemType_prop");
                    var projItemTypes = srcJObj.GetValue("projItemTypeJObj");
                    var projStatusProperties = srcJObj.SelectToken("projStatus.status_Prop");
                    var projStatuses = srcJObj.SelectToken("projStatus").Children().Last().Last();
                    var userItems = srcJObj.GetValue("userDisplayName");
                    var users = ConvertUserDisplay(userItems);

                    var resultItems = ConvertJsonResponseToClass<TaskItem>(itemProperties, items);
                    var resultProjItemTypes = ConvertJsonResponseToClass<ProjectItemType>(projItemTypeProperties, projItemTypes);
                    var resultprojStatuses = ConvertJsonResponseToClass<ProjectStatus>(projStatusProperties, projStatuses);

                    foreach (var resultItem in resultItems)
                    {
                        resultItem.Users = users;
                        resultItem.ProjItemName = resultProjItemTypes.FirstOrDefault(item => item.ProjItemTypeId.Equals(resultItem.ProjItemTypeId)).ItemTypeName;

                        var status = resultprojStatuses.FirstOrDefault(item => item.StatusId.Equals(resultItem.StatusId));
                        resultItem.StatusName = status != null ? status.Name : null;

                        if (resultItem.IsParent)
                        {
                            var urlSubItems = $"team/{teamId}/projects/{resultItem.ProjId}/sprints/{resultItem.SprintId}/item/{resultItem.TaskItemId}/subitem/?action=level";
                            var resSubItems = await client.GetAsync(urlSubItems).ConfigureAwait(false);

                            if (resSubItems.StatusCode == HttpStatusCode.OK)
                            {
                                var resContentSubItems = await resSubItems.Content.ReadAsStringAsync().ConfigureAwait(false);
                                var srcJObjSubItems = JsonConvert.DeserializeObject<JObject>(resContentSubItems);
                                var subIds = srcJObjSubItems.GetValue("itemIds").ToObject<List<string>>();
                                resultItem.SubItemIds = subIds;
                            }
                        }
                    }

                    result.AddRange(resultItems);
                    index += range;
                }
                else
                {
                    hasNext = false;
                }
            }

            return result;
        }

        public async Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(string projectId, IEnumerable<string> itemIds, IEnumerable<string> itemNos)
        {
            var result = new List<TaskItem>();
            var filter = new JObject();
            var url = $"team/{teamId}/projects/{projectId}/item/?action=multipledetails";
            if (itemIds != null && itemIds.Any())
            {
                var itemidarr = HttpUtility.UrlEncode(JsonConvert.SerializeObject(itemIds));
                url += $"&itemidarr={itemidarr}";
            }

            if (itemNos != null && itemNos.Any())
            {
                var itemnoarr = HttpUtility.UrlEncode(JsonConvert.SerializeObject(itemNos));
                url += $"&itemidarr={itemnoarr}";
            }

            var response = await client.GetAsync(url).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(responseContent);
            }

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);

            var itemProperties = srcJObj.GetValue("item_prop");
            var items = srcJObj.GetValue("itemJObj");
            var userItems = srcJObj.GetValue("userDisplayName");
            var users = ConvertUserDisplay(userItems);

            var resultItems = ConvertJsonResponseToClass<TaskItem>(itemProperties, items);

            foreach (var resultItem in resultItems)
            {
                resultItem.Users = users;

                if (resultItem.IsParent)
                {
                    var urlSubItems = $"team/{teamId}/projects/{resultItem.ProjId}/sprints/{resultItem.SprintId}/item/{resultItem.TaskItemId}/subitem/?action=level";
                    var resSubItems = await client.GetAsync(urlSubItems).ConfigureAwait(false);

                    if (resSubItems.StatusCode == HttpStatusCode.OK)
                    {
                        var resContentSubItems = await resSubItems.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var srcJObjSubItems = JsonConvert.DeserializeObject<JObject>(resContentSubItems);
                        var subIds = srcJObjSubItems.GetValue("itemIds").ToObject<List<string>>();
                        resultItem.SubItemIds = subIds;
                    }
                }

                result.AddRange(resultItems);
            }

            result = result.OrderBy(item => item.TaskItemId).ToList();

            return result;
        }

        public async Task UpdateStatusAsync(UpdateItemStatusParameter parameter)
        {
            var url = $"/zsapi/team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/{parameter.TaskItemId}/";
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

        public async Task<TaskItem> CreateAsync(ItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/";
            var formContent = SetAndEncodeParameter(parameter);

            var response = await SendItem(url, formContent).ConfigureAwait(false);
            var result = GetItemFromResponse(response);
            return result.FirstOrDefault();
        }

        public async Task<TaskItem> CreateSubItemAsync(ItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/{parameter.ItemId}/subitem/";
            var formContent = SetAndEncodeParameter(parameter);

            var response = await SendItem(url, formContent).ConfigureAwait(false);
            var result = GetItemFromResponse(response);
            return result.FirstOrDefault();
        }

        public async Task UpdateAsync(ItemSaveParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/{parameter.ItemId}/";
            var formContent = SetAndEncodeParameter(parameter);

            await SendItem(url, formContent).ConfigureAwait(false);
        }

        public async Task DeleteAsync(DeleteItemParameter parameter)
        {
            var url = $"team/{teamId}/projects/{parameter.ProjId}/sprints/{parameter.SprintId}/item/{parameter.ItemId}/";
            var formContent = SetAndEncodeParameter(parameter);

            await SendItem(url, formContent).ConfigureAwait(false);
        }

        private async Task<JObject> SendItem(string url, FormUrlEncodedContent formContent)
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

        private IEnumerable<TaskItem> GetItemFromResponse(JObject srcJObj, Project proj = null)
        {
            var itemProperties = srcJObj.GetValue("item_prop");
            var items = srcJObj.GetValue("itemJObj");
            var userItems = srcJObj.GetValue("userDisplayName");
            var users = ConvertUserDisplay(userItems);

            var resultItems = ConvertJsonResponseToClass<TaskItem>(itemProperties, items);
            return resultItems;
        }
    }
}