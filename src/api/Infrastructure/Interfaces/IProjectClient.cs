using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IProjectClient
    {
        Task<IEnumerable<Project>> SearchAsync();
        
        Task<Project> GetProjectDetailAsync(String no);
    }
}
