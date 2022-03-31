using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class Project
    {
        [JsonProperty(PropertyName = "id")]
        public string? ProjId { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public string? ProjNo { get; set; }

        [JsonProperty(PropertyName = "projName")]
        public string? ProjName { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string? CreatedBy { get; set; }

        public IEnumerable<Sprint>? Sprints { get; set; }
    }
}