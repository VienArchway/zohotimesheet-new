using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class EpicService : IEpicService
    {
        private readonly IEpicClient client;

        public EpicService(IEpicClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<Epic>> SearchAsync(String projId)
        {
            return client.SearchAsync(projId);
        }
    }
}