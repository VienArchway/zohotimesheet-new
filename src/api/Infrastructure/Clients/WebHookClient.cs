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
    public class WebHookClient : ZohoServiceClient, IWebHookClient
    {
        public WebHookClient(HttpClient client, IConfiguration configuration, IServiceProvider svcProvider, IZohoTokenClient zohoTokenClient)
            : base(client, configuration, svcProvider)
        {
            var accessToken = zohoTokenClient.GetAdminAccessTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken.Result.AccessToken);
        }

        public async Task<IEnumerable<WebHook>> GetAllAsync()
        {
            var index = 1;
            var range = 100;
            var hasNext = true;
            var result = new List<WebHook>();

            while (hasNext)
            {
                var response = await client.GetAsync($"team/{teamId}/webhooks/?index={index}&range={range}&viewby=0&action=getwebhooks").ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
                var properties = srcJObj.GetValue("webhooks_prop");
                var items = srcJObj.GetValue("webhooksJObj");
                var resultItems = ConvertJsonResponseToClass<WebHook>(properties, items);

                result.AddRange(resultItems);

                hasNext = srcJObj.GetValue("next").ToObject<bool>();
                index += range;
            }

            return result;
        }

        public async Task UpdateStatusAsync(WebHookStatusUpdateParameter parameter)
        {
            var url = $"team/{teamId}/webhooks/{parameter.WebHookId}/";
            var formContent = SetAndEncodeParameter(parameter);

            client.DefaultRequestHeaders.Add("x-za-reqsize", new string[] { "large" });
            var response = await client.PostAsync(url, formContent).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var srcJObj = JsonConvert.DeserializeObject<JObject>(responseContent);
            var status = srcJObj.GetValue("status")?.ToObject<string>();

            if (status == null || !status.Equals("success"))
            {
                throw new InvalidOperationException(responseContent);
            }
        }
    }
}