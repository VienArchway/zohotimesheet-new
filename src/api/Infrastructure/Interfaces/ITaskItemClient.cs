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
            IEnumerable<String> completedOn,
            int statusId,
            IEnumerable<String> assignees);

        Task<IEnumerable<TaskItem>> SearchByProjectIdAsync(
            String projectId, 
            IEnumerable<String> TaskItemIds, 
            IEnumerable<String> TaskItemNos);

        Task UpdateStatusAsync(UpdateItemStatusParameter parameter);

        Task<TaskItem> CreateAsync(ItemSaveParameter parameter);

        Task<TaskItem> CreateSubItemAsync(ItemSaveParameter parameter);

        Task UpdateAsync(ItemSaveParameter parameter);

        Task DeleteAsync(DeleteItemParameter parameter);
    }
}
