using Newtonsoft.Json;

namespace api.Models
{
    public class Sprint
    {
        [JsonProperty(PropertyName = "id")]
        public String SprintId { get; set; }

        [JsonProperty(PropertyName = "sprintName")]
        public String SprintName { get; set; }

        [JsonProperty(PropertyName = "sprintType")]
        public int? SprintType { get; set; }
    }
}