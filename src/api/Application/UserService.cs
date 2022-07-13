using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class UserService : IUserService
    {

        private readonly ITeamClient teamClient;
        private readonly IUserClient client;

        public UserService(ITeamClient teamClient, IUserClient client)
        {
            this.teamClient = teamClient;
            this.client = client;
        }

        public async Task<User> GetCurrentUser()
        {
            var settingForName =  teamClient.GetTeamSettingAsync("signout");
            var settingForZsUserId = teamClient.GetTeamSettingAsync();
            await Task.WhenAll(settingForName, settingForZsUserId).ConfigureAwait(false);

            var user = new User
            {
                FirstName = settingForName.Result?.FirstName?.Replace(" ",""),
                ZSUserId = settingForZsUserId.Result?.ZsUserId
            };

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await client.GetAllAsync();
        }
    }
}