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
                var projectItems = srcJObj?.GetValue("projectJObj");
                var resultItems = ConvertJsonResponseToClass<Project>(properties, projectItems);

                result.AddRange(resultItems);

                hasNext = srcJObj.GetValue("next").ToObject<bool>();
                index += range;
            }

            return result;
        }
        
        public async Task<Project> GetProjectDetailAsync(String no)
        {
            var result = new Project();

            var response = await client.GetAsync($"team/{teamId}/projects/no-{no}/?action=tabheader").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);

            var projProperties = srcJObj?.GetValue("project_prop");
            var projectItems = srcJObj?.GetValue("projectJObj");
            var projects = ConvertJsonResponseToClass<Project>(projProperties, projectItems);
            result = projects.First();

            var projItemTypeProps = srcJObj?.SelectToken("projItemTypeObj.projItemType_prop");
            var projItemTypeItems = srcJObj?.SelectToken("projItemTypeObj.projItemTypeJObj");
            result.ProjItemTypes = ConvertJsonResponseToClass<ProjectItemType>(projItemTypeProps, projItemTypeItems);

            var projPriorityProps = srcJObj.SelectToken("projPriorityObj.projPriority_prop");
            var projPriorityItems = srcJObj?.SelectToken("projPriorityObj.projPriorityJObj");
            result.ProjPriorities = ConvertJsonResponseToClass<ProjectPriority>(projPriorityProps, projPriorityItems);

            return result;
        }
    }
}