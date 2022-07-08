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
    }
}