using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectItemType
    {
        [JsonProperty(PropertyName = "itemTypeId")]
        public String ItemTypeId { get; set; }

        [JsonProperty(PropertyName = "itemTypeName", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeName { get; set; }
    }
}