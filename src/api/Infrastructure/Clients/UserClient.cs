using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api.Models;
using api.Infrastructure.Interfaces;

namespace api.Infrastructure.Clients
{
    public class UserClient : ZohoServiceClient, IUserClient
    {
        public UserClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var response = await client.GetAsync($"team/{teamId}/users/allzuid/?action=zpuidVSzuid").ConfigureAwait(false);
            // var response = await client.GetAsync($"team/{teamId}/globalview/?action=users").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var properties = new JObject();
            properties.Add("displayName", 0);
            properties.Add("emailId", 1);
            properties.Add("userRole", 2);
            var items = srcJObj.GetValue("userIdvsDetails");
            var result = ConvertJsonResponseToClass<User>(properties, items);

            return result;
        }

        public async Task<string> GetZSUserIdIdByUserIdAsync(string userId)
        {
            var response = await client.GetAsync($"team/{teamId}/users/allzuid/?action=zpuidVSzuid").ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var items = (JObject)srcJObj.GetValue("zsuserIdvsIAMUserId");
            var result = items.Properties().FirstOrDefault(x => x.HasValues && x.Value.ToString() == userId);
            return result.Name;
        }
    }
}