using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IItemClient
    {
        Task<IEnumerable<Item>> SearchAsync(
            DateTime? startDateFrom,
            DateTime? startDateTo,
            IEnumerable<int> sprintTypeIds,
            IEnumerable<string> completedOn,
            int statusId,
            IEnumerable<string> assignees);

        Task<IEnumerable<Item>> SearchByProjectIdAsync(
            string projectId, 
            IEnumerable<string> itemIds, 
            IEnumerable<string> itemNos);

        Task UpdateStatusAsync(UpdateItemStatusParameter parameter);

        Task<Item> CreateAsync(ItemSaveParameter parameter);

        Task<Item> CreateSubItemAsync(ItemSaveParameter parameter);

        Task UpdateAsync(ItemSaveParameter parameter);

        Task DeleteAsync(DeleteItemParameter parameter);
    }
}
