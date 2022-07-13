using api.Models;

namespace api.Application.Interfaces
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItem>> SearchAsync(TaskItemSearchParameter parameter);

        Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(String projectId, TaskItemSearchParameter parameter);

        Task UpdateStatusAsync(UpdateItemStatusParameter parameter);

        Task<TaskItem> CreateAsync(ItemSaveParameter parameter);

        Task<TaskItem> CreateSubItemAsync(ItemSaveParameter parameter);

        Task UpdateAsync(ItemSaveParameter parameter);

        Task DeleteAsync(DeleteItemParameter parameter);
    }
}