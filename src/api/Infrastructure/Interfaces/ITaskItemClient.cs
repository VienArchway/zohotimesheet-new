using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ITaskItemClient
    {
        Task<IEnumerable<TaskItem>> SearchAsync(
            DateTime? startDateFrom,
            DateTime? startDateTo,
            IEnumerable<int> sprintTypeIds,
            IEnumerable<string> completedOn,
            int statusId,
            IEnumerable<string> assignees);

        Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(
            string projectId, 
            IEnumerable<string> TaskItemIds, 
            IEnumerable<string> TaskItemNos);

        Task UpdateStatusAsync(UpdateTaskItemStatusParameter parameter);

        Task<TaskItem> CreateAsync(TaskItemSaveParameter parameter);

        Task<TaskItem> CreateSubTaskItemAsync(TaskItemSaveParameter parameter);

        Task UpdateAsync(TaskItemSaveParameter parameter);

        Task DeleteAsync(DeleteTaskItemParameter parameter);
    }
}
