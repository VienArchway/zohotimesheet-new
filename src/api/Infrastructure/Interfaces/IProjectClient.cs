using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IProjectClient
    {
        Task<IEnumerable<Project>> SearchAsync();

        Task<IEnumerable<ProjectPriority>> GetProjectPriorityAsync(String id);

        Task<IEnumerable<ProjectItemType>> GetProjectItemTypeAsync(String id);

        Task<Project> GetProjectDetailAsync(String no);
    }
}
