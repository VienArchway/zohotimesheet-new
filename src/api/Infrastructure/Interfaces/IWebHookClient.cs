using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IWebHookClient
    {
        Task<IEnumerable<WebHook>> GetAllAsync();

        Task UpdateStatusAsync(WebHookStatusUpdateParameter parameter);
    }
}
