using System.Net.Http.Headers;
using System.Text.Json;
using api.Infrastructure.Interfaces;

namespace api.Infrastructure.Clients
{
    public class TeamClient : ZohoServiceClient, ITeamClient
    {
        public TeamClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public record TeamSetting
        {
            public string? FirstName { get; set; }
            public string? ZsUserId { get; set; }
        }

        public async Task<TeamSetting?> GetTeamSettingAsync(string? action = null, string? accessToken = null)
        {
            if (!string.IsNullOrEmpty(accessToken)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
            }
            
            using var response = await client.GetAsync($"team/{teamId}/settings/?action=" + action, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode) return null;
            try
            {
                if (response.Content != null)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<TeamSetting?>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            finally
            {
                response.Dispose();
            }

            return null;
        }

        public async Task<string?> FetchTeamsAsync()
        {
            using var response = await client.GetAsync($"teams/", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            return !response.IsSuccessStatusCode ? null : "success";
        }
    }
}