using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectStatus
    {
        [JsonProperty(PropertyName = "statusId", NullValueHandling=NullValueHandling.Ignore)]
        public String StatusId { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling=NullValueHandling.Ignore)]
        public String Color { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling=NullValueHandling.Ignore)]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling=NullValueHandling.Ignore)]
        public int Type { get; set; }
    }
}