using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class UserService : IUserService
    {
        private readonly IUserClient client;

        public UserService(IUserClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return client.GetAllAsync();
        }

        public Task<string> GetZSUserIdIdByUserIdAsync(string userId)
        {
            return client.GetZSUserIdIdByUserIdAsync(userId);
        }
    }
}