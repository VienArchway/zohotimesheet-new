using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class SprintService : ISprintService
    {
        private readonly ISprintClient client;

        public SprintService(ISprintClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<Sprint>> SearchAsync(SprintSearchParameter parameter)
        {
            return client.SearchAsync(parameter.ProjectId, parameter.SprintTypeId);
        }
    }
}