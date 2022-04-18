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
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace api.Infrastructure.Clients
{
    public class TeamClient : ZohoServiceClient, ITeamClient
    {
        public TeamClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<Team>> SearchAsync()
        {
            var response = await client.GetAsync("teams/").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            
            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (!response.IsSuccessStatusCode)
            {
                return new List<Team>();
            }
            var portals = srcJObj?.SelectToken("portals") as JArray;
            return portals?.ToObject<List<Team>>() ?? new List<Team>();
        }
    }
}