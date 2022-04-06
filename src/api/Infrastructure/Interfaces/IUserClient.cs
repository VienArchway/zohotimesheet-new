using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IUserClient
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<string> GetZSUserIdIdByUserIdAsync(string userId);
    }
}
