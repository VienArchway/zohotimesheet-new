using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ISprintClient
    {
        Task<IEnumerable<Sprint>> SearchAsync(string projectId, int? sprintTypeId);
    }
}
