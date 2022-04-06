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
using System.Net;

namespace api.Infrastructure.Clients
{
    public class SprintClient : ZohoServiceClient, ISprintClient
    {
        public SprintClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<Sprint>> SearchAsync(string projectId, int? sprintTypeId)
        {
            var result = new List<Sprint>();
            var sprintType = sprintTypeId != null && sprintTypeId != 0 ? $"%5B{sprintTypeId}%5D" : "%5B2,3,4%5D";
            var response = await client.GetAsync($"team/{teamId}/projects/{projectId}/sprints/?action=data&index=1&range=100&type={sprintType}").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(responseContent);
            }
            else
            {
                var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                var properties = srcJObj.GetValue("sprint_prop");
                var items = srcJObj.GetValue("sprintJObj");
                result.AddRange(ConvertJsonResponseToClass<Sprint>(properties, items));
            }

            return result;
        }
    }
}