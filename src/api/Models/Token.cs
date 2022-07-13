using System.Text.Json.Serialization;

namespace api.Models
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public String AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public String RefreshToken { get; set; }
    }
}