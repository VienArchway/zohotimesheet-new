using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class WebHookStatusUpdateParameter
    {
        public String WebHookId { get; set; }
        
        [JsonProperty(PropertyName = "action")]
        public String Action { get; set; }

        [JsonProperty(PropertyName = "hookstatus")]
        public int HookStatus { get; set; }

    }
}