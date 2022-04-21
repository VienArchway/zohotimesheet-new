using api.Infrastructure.Interfaces;
using api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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