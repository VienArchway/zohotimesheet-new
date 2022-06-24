using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class UserService : IUserService
    {

        private readonly ITeamClient teamClient;

        public UserService(ITeamClient teamClient)
        {
            this.teamClient = teamClient;
        }

        public async Task<User> GetCurrentUser()
        {
            var settingForName =  await teamClient.GetTeamSettingAsync("signout").ConfigureAwait(false);

            var settingForZsUserId = await teamClient.GetTeamSettingAsync().ConfigureAwait(false);

            var user = new User();
            user.FirstName = settingForName?["firstName"]?.ToString();
            user.ZSUserId = settingForZsUserId?["zsuserId"]?.ToString();

            return user;
        }
    }
}