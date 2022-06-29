using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class LogWorkService : ILogWorkService
    {
        private readonly ILogWorkClient client;

        private readonly IProjectClient projectClient;

        private readonly ISprintClient sprintClient;

        private readonly IBackLogClient backlogClient;

        public LogWorkService(
            ILogWorkClient client,
            IProjectClient projectClient,
            ISprintClient sprintClient,
            IBackLogClient backlogClient)
        {
            this.client = client;
            this.projectClient = projectClient;
            this.sprintClient = sprintClient;
            this.backlogClient = backlogClient;
        }

        public async Task<IEnumerable<LogWork>> SearchAsync(LogWorkSearchParameter parameter)
        {
            var projects = parameter.Projects;
            var delaySeconds = projects != null && projects.Count() > 6 ? 10 : 0;
            var projectIds = projects != null ? projects.Select(x => x.ProjId) : null;
            var result = await client.SearchAsync(parameter.StartDate, parameter.EndDate, projectIds, parameter.SprintTypes, parameter.OwnerIds).ConfigureAwait(false);

            return result;
        }

        public Task<LogWork> CreateAsync(LogWorkSaveParameter parameter)
        {
            return client.CreateAsync(parameter);
        }

        public Task UpdateAsync(LogWorkSaveParameter parameter)
        {
            return client.UpdateAsync(parameter);
        }
    }
}