using api.Models;

namespace api.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<string> GetZSUserIdIdByUserIdAsync(string userId);
    }
}