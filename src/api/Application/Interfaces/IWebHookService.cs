using api.Models;

namespace api.Application.Interfaces
{
    public interface IWebHookService
    {
        Task<IEnumerable<WebHook>> GetAllAsync();

        Task UpdateStatusAsync(WebHookStatusUpdateParameter parameter);
    }
}