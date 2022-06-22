using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class UserService : IUserService
    {
        private readonly IUserClient client;

        private readonly ITeamClient teamClient;

        public UserService(IUserClient client, ITeamClient teamClient)
        {
            this.client = client;
            this.teamClient = teamClient;
        }

        public async Task<User> GetCurrentUser()
        {
            var zohoUser = await client.GetCurrentZohoUser();

            var sprintSetting = await teamClient.GetTeamSettingAsync().ConfigureAwait(false);

            var user = new User();
            user.DisplayName = zohoUser?.DisplayName;
            user.EmailId = zohoUser?.Email;
            user.FirstName = zohoUser?.FirstName;
            user.ZSUserId = sprintSetting["zsuserId"].ToString();

            return user;
        }
    }
}