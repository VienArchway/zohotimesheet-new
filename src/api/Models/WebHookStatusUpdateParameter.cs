using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class WebHookStatusUpdateParameter
    {
        public string WebHookId { get; set; }
        
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "hookstatus")]
        public int HookStatus { get; set; }

    }
}