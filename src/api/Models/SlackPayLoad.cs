using Newtonsoft.Json;

namespace api.Models
{
    public class SlackPayLoad
    {
        [JsonProperty("channel")]
        public String Channel { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("text")]
        public String Text { get; set; }
    }
}
