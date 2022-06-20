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

        public async Task<string?> SearchAsync()
        {
            var response = await client.GetAsync("teams/").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            return !response.IsSuccessStatusCode ? string.Empty : srcJObj?.GetValue("status")?.ToString();
        }

        public async Task<string?> GetDisplayNameAsync()
        {
            var resUser = await client.GetAsync(configuration["Zoho:UserHost"]);
            if (!resUser.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Has error when get displayName");
            }
            var userInfo = await resUser.Content.ReadAsStringAsync().ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<ZohoUser>(userInfo);
            Environment.SetEnvironmentVariable("displayName", user.DisplayName);
            return user.DisplayName ?? throw new InvalidOperationException();
        }
    }
}