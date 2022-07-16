using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectItemType
    {
        [JsonProperty(PropertyName = "id", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeId { get; set; }

        [JsonProperty(PropertyName = "itemTypeName", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeName { get; set; }

        [JsonProperty(PropertyName = "baseType", NullValueHandling=NullValueHandling.Ignore)]
        public int BaseType { get; set; }
    }
}