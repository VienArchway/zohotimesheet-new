using Newtonsoft.Json;

namespace api.Models
{
    public class Sprint
    {
        [JsonProperty(PropertyName = "id")]
        public string? SprintId { get; set; }

        [JsonProperty(PropertyName = "sprintName")]
        public string? SprintName { get; set; }

        [JsonProperty(PropertyName = "sprintType")]
        public int? SprintType { get; set; }
    }
}