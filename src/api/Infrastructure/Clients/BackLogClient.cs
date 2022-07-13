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
    public class BackLogClient : ZohoServiceClient, IBackLogClient
    {
        public BackLogClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<BackLog> SearchAsync(String projectId)
        {
            var response = await client.GetAsync($"team/{teamId}/projects/{projectId}/?action=getbacklog").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            BackLog result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(responseContent);
            }
            else
            {
                result = JsonConvert.DeserializeObject<BackLog>(responseContent);
            }

            return result;
        }
    }
}