using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IBackLogClient
    {
        Task<BackLog> SearchAsync(String projectId);
    }
}
