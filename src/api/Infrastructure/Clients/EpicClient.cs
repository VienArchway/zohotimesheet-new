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
    public class EpicClient : ZohoServiceClient, IEpicClient
    {
        public EpicClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<Epic>> SearchAsync(String projId)
        {
            var index = 1;
            var range = 100;
            var hasNext = true;
            var result = new List<Epic>();

            while (hasNext)
            {
                var response = await client.GetAsync($"team/{teamId}/projects/{projId}/epic/?action=data&index={index}&range={range}").ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                var properties = srcJObj?.GetValue("epic_prop");
                var epicItems = srcJObj?.GetValue("epicJObj");
                var resultItems = ConvertJsonResponseToClass<Epic>(properties, epicItems);

                result.AddRange(resultItems);

                hasNext = srcJObj.GetValue("next").ToObject<bool>();
                index += range;
            }

            return result;
        }
    }
}