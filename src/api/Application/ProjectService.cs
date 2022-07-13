using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectClient client;

        public ProjectService(IProjectClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<Project>> SearchAsync()
        {
            return client.SearchAsync();
        }

        public Task<IEnumerable<ProjectPriority>> GetProjectPriorityAsync(String id)
        {
            return client.GetProjectPriorityAsync(id);
        }

        public Task<IEnumerable<ProjectItemType>> GetProjectItemTypeAsync(String id)
        {
            return client.GetProjectItemTypeAsync(id);
        }
    }
}