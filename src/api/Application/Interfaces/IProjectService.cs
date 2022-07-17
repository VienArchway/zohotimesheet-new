using api.Models;

namespace api.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> SearchAsync();
        
        Task<IEnumerable<ProjectPriority>> GetProjectPriorityAsync(String id);

        Task<IEnumerable<ProjectItemType>> GetProjectItemTypeAsync(String id);

        Task<Project> GetProjectDetailAsync(String no);
    }
}