using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class ItemService : IItemService
    {
        private readonly IItemClient client;

        public ItemService(IItemClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<Item>> SearchAsync(ItemSearchParameter parameter)
        {
            return client.SearchAsync(
                parameter.StartDateFrom,
                parameter.StartDateTo,
                parameter.SprintTypeIds,
                parameter.CompletedOn,
                parameter.StatusId,
                parameter.Assignees);
        }

        public Task<IEnumerable<Item>> SearchByProjectIdAsync(string projectId, ItemSearchParameter parameter)
        {
            return client.SearchByProjectIdAsync(
                projectId,
                parameter.itemIds,
                parameter.itemNos);
        }

        public Task UpdateStatusAsync(UpdateItemStatusParameter parameter)
        {
            return client.UpdateStatusAsync(parameter);
        }

        public Task<Item> CreateAsync(ItemSaveParameter parameter)
        {
            return client.CreateAsync(parameter);
        }

        public Task<Item> CreateSubItemAsync(ItemSaveParameter parameter)
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