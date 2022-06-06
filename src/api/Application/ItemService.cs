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

        public Task UpdateStatusAsync(UpdateItemStatusParameter parameter)
        {
            return client.UpdateStatusAsync(parameter);
        }

        public Task<TaskItem> CreateAsync(ItemSaveParameter parameter)
        {
            return client.CreateAsync(parameter);
        }

        public Task<TaskItem> CreateSubTaskItemAsync(ItemSaveParameter parameter)
        {
            return client.CreateSubItemAsync(parameter);
        }

        public Task UpdateAsync(ItemSaveParameter parameter)
        {
            return client.UpdateAsync(parameter);
        }

        public Task DeleteAsync(DeleteItemParameter parameter)
        {
            return client.DeleteAsync(parameter);
        }
    }
}