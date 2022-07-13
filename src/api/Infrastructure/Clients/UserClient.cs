using api.Infrastructure.Interfaces;
using api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.Infrastructure.Clients
{
    public class UserClient : ZohoServiceClient, IUserClient
    {
        public UserClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
            path = $"team/{teamId}/users/";
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var response = await client.GetAsync($"{path}?action=data&index=1&range=100").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var properties = srcJObj?.GetValue("user_prop");
            var items = srcJObj?.GetValue("userJObj");
            var result = ConvertJsonResponseToClass<User>(properties, items);

            return result;
        }
    }
}