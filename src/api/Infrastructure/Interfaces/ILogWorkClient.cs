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
            IEnumerable<string> projectIds,
            IEnumerable<string> sprintTypes,
            IEnumerable<string> ownerIds,
            int delayTimeoutBySeconds = 0);

        Task<IEnumerable<LogWork>> SearchByGlobalViewAsync(
            DateTime? logdateFrom,
            DateTime? logdateTo,
            IEnumerable<string> projectIds,
            IEnumerable<string> ownerIds,
            int delayTimeoutBySeconds = 0);

        Task<LogWork> CreateAsync(LogWorkSaveParameter parameter);

        Task UpdateAsync(LogWorkSaveParameter parameter);
    }
}
