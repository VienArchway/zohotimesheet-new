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
            var result = new List<LogWork>();
            var projects = (parameter.Projects != null && parameter.Projects.Any()) ? parameter.Projects :
                    await projectClient.SearchAsync().ConfigureAwait(false);

            foreach (var project in projects)
            {
                var sprints = project.Sprints;
                if (sprints == null || !sprints.Any())
                {
                    sprints = await sprintClient.SearchAsync(project.ProjId, parameter.SprintTypeId).ConfigureAwait(false);
                }

                if (parameter.SprintTypeId == null || parameter.SprintTypeId == 0)
                {
                    var backlog = await backlogClient.SearchAsync(project.ProjId).ConfigureAwait(false);
                    if (backlog != null)
                    {
                        sprints = sprints.Append(new Sprint() { SprintId = backlog.BacklogId, SprintName = "BackLog" });
                    }
                }

                var delaySeconds = sprints.Count() > 10 ? 3 : 0;

                foreach (var sprint in sprints)
                {
                    result.AddRange(await client.SearchAsync(parameter.StartDate, parameter.EndDate, project, sprint, parameter.OwnerIds, delaySeconds).ConfigureAwait(false));
                }
            }

            return result;
        }

        public async Task<IEnumerable<LogWork>> SearchByGlobalViewAsync(LogWorkSearchParameter parameter)
        {
            var projects = parameter.Projects;
            var delaySeconds = projects != null && projects.Count() > 6 ? 10 : 0;
            var projectIds = projects != null ? projects.Select(x => x.ProjId) : null;
            var result = await client.SearchByGlobalViewAsync(parameter.StartDate, parameter.EndDate, projectIds, parameter.OwnerIds, delaySeconds).ConfigureAwait(false);
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