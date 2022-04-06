using api.Models;

namespace api.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> SearchAsync();
        
        Task<IEnumerable<ProjectPriority>> GetProjectPriorityAsync(string id);

        Task<IEnumerable<ProjectItemType>> GetProjectItemTypeAsync(string id);
    }
}