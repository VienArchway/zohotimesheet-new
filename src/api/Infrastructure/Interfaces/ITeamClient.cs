using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ITeamClient
    {
        Task<IEnumerable<Team>> SearchAsync();
    }
}
