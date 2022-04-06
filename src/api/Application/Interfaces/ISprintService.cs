using api.Models;

namespace api.Application.Interfaces
{
    public interface ISprintService
    {
       Task<IEnumerable<Sprint>> SearchAsync(SprintSearchParameter parameter);
    }
}