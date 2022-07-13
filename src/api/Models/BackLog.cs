using Newtonsoft.Json;

namespace api.Models
{
    public class BackLog
    {
        [JsonProperty(PropertyName = "backlogId")]
        public String BacklogId { get; set; }
    }
}