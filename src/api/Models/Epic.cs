using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class Epic
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "epicNo")]
        public String No { get; set; }

        [JsonProperty(PropertyName = "epicName")]
        public String Name { get; set; }
    }
}