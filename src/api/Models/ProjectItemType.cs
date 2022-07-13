using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectItemType
    {
        [JsonProperty(PropertyName = "id")]
        public String ProjId { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId", NullValueHandling=NullValueHandling.Ignore)]
        public String ProjItemTypeId { get; set; }

        [JsonProperty(PropertyName = "ItemTypeImage", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeImage { get; set; }

        [JsonProperty(PropertyName = "baseType", NullValueHandling=NullValueHandling.Ignore)]
        public int BaseType { get; set; }

        [JsonProperty(PropertyName = "ItemTypeId", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeId { get; set; }

        [JsonProperty(PropertyName = "ItemTypeName", NullValueHandling=NullValueHandling.Ignore)]
        public String ItemTypeName { get; set; }
    }
}