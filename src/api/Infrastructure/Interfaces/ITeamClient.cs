using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ITeamClient
    {
        Task<string?> SearchAsync();

        Task<string?> GetDisplayNameAsync();
    }
}
