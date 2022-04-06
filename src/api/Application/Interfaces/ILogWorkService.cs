using api.Models;

namespace api.Application.Interfaces
{
    public interface ILogWorkService
    {
        Task<IEnumerable<LogWork>> SearchAsync(LogWorkSearchParameter parameter);

        Task<IEnumerable<LogWork>> SearchByGlobalViewAsync(LogWorkSearchParameter parameter);

        Task<LogWork> CreateAsync(LogWorkSaveParameter parameter);

        Task UpdateAsync(LogWorkSaveParameter parameter);
    }
}