using TeamClient = api.Infrastructure.Clients.TeamClient;

namespace api.Infrastructure.Interfaces
{
    public interface ITeamClient
    {
        Task<TeamClient.TeamSetting> GetTeamSettingAsync(String action = null, String accessToken = null);
        Task<String> FetchTeamsAsync();
    }
}
