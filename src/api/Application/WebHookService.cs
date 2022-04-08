using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class WebHookService : IWebHookService
    {
        private readonly IWebHookClient client;

        public WebHookService(IWebHookClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<WebHook>> GetAllAsync()
        {
            return client.GetAllAsync();
        }

        public async Task UpdateStatusAsync(WebHookStatusUpdateParameter parameter)
        {
            await client.UpdateStatusAsync(parameter).ConfigureAwait(false);
        }
    }
}