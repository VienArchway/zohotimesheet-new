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
using System.Net.Http.Headers;

namespace api.Infrastructure.Clients
{
    public class UserClient : ZohoServiceClient, IUserClient
    {
        public UserClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider)
            : base(client, configuration, svcProvider)
        {
        }

        public async Task<ZohoUser> GetCurrentZohoUser(string accessToken = null)
        {
            if (!String.IsNullOrEmpty(accessToken)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
            }

            var resUser = await client.GetAsync(configuration["Zoho:UserHost"]).ConfigureAwait(false);
            if (!resUser.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Has error when get displayName");
            }
            var userInfo = await resUser.Content.ReadAsStringAsync().ConfigureAwait(false);
            var zohoUser = JsonConvert.DeserializeObject<ZohoUser>(userInfo);

            return zohoUser;
        }
    }
}