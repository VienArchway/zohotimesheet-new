using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class Project
    {
        [JsonProperty(PropertyName = "id")]
        public String ProjId { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public String ProjNo { get; set; }

        [JsonProperty(PropertyName = "projName")]
        public String ProjName { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public String CreatedBy { get; set; }

        public IEnumerable<Sprint> Sprints { get; set; }
    }
}