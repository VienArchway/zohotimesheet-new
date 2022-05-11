using api.Infrastructure.Interfaces;
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

        public async Task<string?> SearchAsync()
        {
            var response = await client.GetAsync("teams/").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            return !response.IsSuccessStatusCode ? string.Empty : srcJObj?.GetValue("status")?.ToString();
        }
    }
}