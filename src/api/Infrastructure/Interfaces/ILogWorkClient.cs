using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ILogWorkClient
    {
        Task<IEnumerable<LogWork>> SearchAsync(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<String> projectIds,
            IEnumerable<String> sprintTypes,
            IEnumerable<String> ownerIds,
            int delayTimeoutBySeconds = 0);

        Task<LogWork> CreateAsync(LogWorkSaveParameter parameter);

        Task UpdateAsync(LogWorkSaveParameter parameter);
    }
}
