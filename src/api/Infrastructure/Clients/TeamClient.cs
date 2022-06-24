using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
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

        public async Task<JObject?> GetTeamSettingAsync(string? action = null, string? accessToken = null)
        {
            if (!string.IsNullOrEmpty(accessToken)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
            }

            var response = await client.GetAsync($"team/{teamId}/settings/?action=" + action).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<JObject>(responseContent);
            return result;
        }
    }
}