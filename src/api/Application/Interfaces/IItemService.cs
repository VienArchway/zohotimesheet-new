using api.Models;

namespace api.Application.Interfaces
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItem>> SearchAsync(TaskItemSearchParameter parameter);

        Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(string projectId, TaskItemSearchParameter parameter);

        Task UpdateStatusAsync(UpdateItemStatusParameter parameter);

        Task<TaskItem> CreateAsync(ItemSaveParameter parameter);

        Task<TaskItem> CreateSubTaskItemAsync(ItemSaveParameter parameter);

        Task UpdateAsync(ItemSaveParameter parameter);

        Task DeleteAsync(DeleteItemParameter parameter);
    }
}