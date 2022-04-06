using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ProjectItemType
    {
        [JsonProperty(PropertyName = "id")]
        public string ProjId { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId", NullValueHandling=NullValueHandling.Ignore)]
        public string ProjItemTypeId { get; set; }

        [JsonProperty(PropertyName = "ItemTypeImage", NullValueHandling=NullValueHandling.Ignore)]
        public string ItemTypeImage { get; set; }

        [JsonProperty(PropertyName = "baseType", NullValueHandling=NullValueHandling.Ignore)]
        public int BaseType { get; set; }

        [JsonProperty(PropertyName = "ItemTypeId", NullValueHandling=NullValueHandling.Ignore)]
        public string ItemTypeId { get; set; }

        [JsonProperty(PropertyName = "ItemTypeName", NullValueHandling=NullValueHandling.Ignore)]
        public string ItemTypeName { get; set; }
    }
}