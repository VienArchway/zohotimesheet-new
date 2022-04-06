using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class TaskItemSaveParameter
    {
        public string TaskItemId { get; set; }

        public string ProjId { get; set; }

        public string SprintId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "projItemTypeid", NullValueHandling=NullValueHandling.Ignore)]
        public long? ProjItemTypeId { get; set; }

        [JsonProperty(PropertyName = "projpriorityid", NullValueHandling=NullValueHandling.Ignore)]
        public long? ProjPriorityId { get; set; }

        [JsonProperty(PropertyName = "users", NullValueHandling=NullValueHandling.Ignore)]
        public string[] Users  { get; set; }

        [JsonProperty(PropertyName = "description", NullValueHandling=NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "point", NullValueHandling=NullValueHandling.Ignore)]
        public int? Point { get; set; }

        [JsonProperty(PropertyName = "duration", NullValueHandling=NullValueHandling.Ignore)]
        public string duration { get; set; }

        [JsonProperty(PropertyName = "startdate", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty(PropertyName = "enddate", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
    }
}