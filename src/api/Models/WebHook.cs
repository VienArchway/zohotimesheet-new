using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class WebHook
    {
        [JsonProperty(PropertyName = "id")]
        public string WebHookId { get; set; }

        [JsonProperty(PropertyName = "webhookName")]
        public string WebHookName { get; set; }

        [JsonProperty(PropertyName = "webhookStatus")]
        public int WebHookStatus { get; set; }

        [JsonProperty(PropertyName = "webhookUrl")]
        public string WebHookUrl { get; set; }
    }
}