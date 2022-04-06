using Newtonsoft.Json;

namespace api.Models
{
    public class BackLog
    {
        [JsonProperty(PropertyName = "backlogId")]
        public string BacklogId { get; set; }
    }
}