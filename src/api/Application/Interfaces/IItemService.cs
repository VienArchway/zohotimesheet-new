using api.Models;

namespace api.Application.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> SearchAsync(ItemSearchParameter parameter);

        Task<IEnumerable<Item>> SearchByProjectIdAsync(string projectId, ItemSearchParameter parameter);

        Task UpdateStatusAsync(UpdateItemStatusParameter parameter);

        Task<Item> CreateAsync(ItemSaveParameter parameter);

        Task<Item> CreateSubItemAsync(ItemSaveParameter parameter);

        Task UpdateAsync(ItemSaveParameter parameter);

        Task DeleteAsync(DeleteItemParameter parameter);
    }
}