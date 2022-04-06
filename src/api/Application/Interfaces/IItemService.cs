using api.Models;

namespace api.Application.Interfaces
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItem>> SearchAsync(TaskItemSearchParameter parameter);

        Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(string projectId, TaskItemSearchParameter parameter);

        Task UpdateStatusAsync(UpdateTaskItemStatusParameter parameter);

        Task<TaskItem> CreateAsync(TaskItemSaveParameter parameter);

        Task<TaskItem> CreateSubTaskItemAsync(TaskItemSaveParameter parameter);

        Task UpdateAsync(TaskItemSaveParameter parameter);

        Task DeleteAsync(DeleteTaskItemParameter parameter);
    }
}