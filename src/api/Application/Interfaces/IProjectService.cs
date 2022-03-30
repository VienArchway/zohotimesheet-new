using api.Models;

namespace api.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> SearchAsync();
    }
}