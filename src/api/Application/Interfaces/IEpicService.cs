using api.Models;

namespace api.Application.Interfaces
{
    public interface IEpicService
    {
        Task<IEnumerable<Epic>> SearchAsync(String projId);
    }
}