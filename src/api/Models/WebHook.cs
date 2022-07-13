using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class WebHook
    {
        [JsonProperty(PropertyName = "id")]
        public String WebHookId { get; set; }

        [JsonProperty(PropertyName = "webhookName")]
        public String WebHookName { get; set; }

        [JsonProperty(PropertyName = "webhookStatus")]
        public int WebHookStatus { get; set; }

        [JsonProperty(PropertyName = "webhookUrl")]
        public String WebHookUrl { get; set; }
    }
}