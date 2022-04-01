using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string? RefreshToken { get; set; }
    }
}