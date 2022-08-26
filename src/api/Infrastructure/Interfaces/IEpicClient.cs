using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IEpicClient
    {
        Task<IEnumerable<Epic>> SearchAsync(String projId);
    }
}
