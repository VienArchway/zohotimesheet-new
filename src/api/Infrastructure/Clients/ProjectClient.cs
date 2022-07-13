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

namespace api.Infrastructure.Clients
{
    public class ProjectClient : ZohoServiceClient, IProjectClient
    {
        public ProjectClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<Project>> SearchAsync()
        {
            var index = 1;
            var range = 100;
            var hasNext = true;
            var result = new List<Project>();

            while (hasNext)
            {
                var response = await client.GetAsync($"team/{teamId}/projects/?action=allprojects&index={index}&range={range}&viewby=0").ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                var properties = srcJObj?.GetValue("project_prop");
                var TaskItems = srcJObj?.GetValue("projectJObj");
                var resultTaskItems = ConvertJsonResponseToClass<Project>(properties, TaskItems);

                result.AddRange(resultTaskItems);

                hasNext = srcJObj.GetValue("next").ToObject<bool>();
                index += range;
            }

            return result;
        }

        public async Task<IEnumerable<ProjectPriority>> GetProjectPriorityAsync(String id)
        {
            var result = new List<ProjectPriority>();

            var response = await client.GetAsync($"team/{teamId}/projects/{id}/priority/").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var properties = srcJObj.GetValue("projPriority_prop");
            var items = srcJObj.GetValue("projPriorityJObj");
            var resultItems = ConvertJsonResponseToClass<ProjectPriority>(properties, items);

            result.AddRange(resultItems);

            return result;
        }

        public async Task<IEnumerable<ProjectItemType>> GetProjectItemTypeAsync(String id)
        {
            var result = new List<ProjectItemType>();

            var response = await client.GetAsync($"team/{teamId}/projects/{id}/itemtype/?action=alldata").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var properties = srcJObj.GetValue("projItemType_prop");
            var items = srcJObj.GetValue("projItemTypeJObj");
            var resultItems = ConvertJsonResponseToClass<ProjectItemType>(properties, items);

            result.AddRange(resultItems);

            return result;
        }
    }
}