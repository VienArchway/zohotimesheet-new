using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemClient client;

        public TaskItemService(ITaskItemClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<TaskItem>> SearchAsync(TaskItemSearchParameter parameter)
        {
            return client.SearchAsync(
                parameter.StartDateFrom,
                parameter.StartDateTo,
                parameter.SprintTypeIds,
                parameter.CompletedOn,
                parameter.StatusId,
                parameter.Assignees);
        }

        public Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(string projectId, TaskItemSearchParameter parameter)
        {
            return client.SearchByProjectIdAsync(
                projectId,
                parameter.TaskItemIds,
                parameter.TaskItemNos);
        }

        public Task UpdateStatusAsync(UpdateTaskItemStatusParameter parameter)
        {
            return client.UpdateStatusAsync(parameter);
        }

        public Task<TaskItem> CreateAsync(TaskItemSaveParameter parameter)
        {
            return client.CreateAsync(parameter);
        }

        public Task<TaskItem> CreateSubTaskItemAsync(TaskItemSaveParameter parameter)
        {
            return client.CreateSubTaskItemAsync(parameter);
        }

        public Task UpdateAsync(TaskItemSaveParameter parameter)
        {
            return client.UpdateAsync(parameter);
        }

        public Task DeleteAsync(DeleteTaskItemParameter parameter)
        {
            return client.DeleteAsync(parameter);
        }
    }
}