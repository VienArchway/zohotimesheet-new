using api.Models;

namespace api.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrentUser();
        Task<IEnumerable<User>> GetAllAsync();
    }
}