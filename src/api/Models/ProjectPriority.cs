using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectPriority
    {
        [JsonProperty(PropertyName = "id")]
        public String PriorityId { get; set; }

        [JsonProperty(PropertyName = "priorityName")]
        public String PriorityName { get; set; }
    }
}