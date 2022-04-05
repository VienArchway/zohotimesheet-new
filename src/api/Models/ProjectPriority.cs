using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectPriority
    {
        [JsonProperty(PropertyName = "id")]
        public string ProjId { get; set; }

        [JsonProperty(PropertyName = "priorityId")]
        public string PriorityId { get; set; }

        [JsonProperty(PropertyName = "priorityName")]
        public string PriorityName { get; set; }

        [JsonProperty(PropertyName = "isDefault")]
        public string IsDefault { get; set; }

        [JsonProperty(PropertyName = "colorCode")]
        public string ColorCode { get; set; }
    }
}