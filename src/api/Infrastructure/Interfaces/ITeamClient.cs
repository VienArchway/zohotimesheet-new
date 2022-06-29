using TeamClient = api.Infrastructure.Clients.TeamClient;

namespace api.Infrastructure.Interfaces
{
    public interface ITeamClient
    {
        Task<TeamClient.TeamSetting?> GetTeamSettingAsync(string? action = null, string? accessToken = null);
        Task<string?> FetchTeamsAsync();
    }
}
