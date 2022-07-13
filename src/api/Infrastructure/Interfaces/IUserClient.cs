using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IUserClient
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
}
