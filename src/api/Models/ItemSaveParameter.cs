using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ItemSaveParameter
    {
        public String Id { get; set; }

        public String ProjId { get; set; }

        public String SprintId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String ItemName { get; set; }

        [JsonProperty(PropertyName = "projitemtypeid", NullValueHandling=NullValueHandling.Ignore)]
        public long? ProjItemTypeId { get; set; }

        [JsonProperty(PropertyName = "projpriorityid", NullValueHandling=NullValueHandling.Ignore)]
        public long? ProjPriorityId { get; set; }

        [JsonProperty(PropertyName = "users", NullValueHandling=NullValueHandling.Ignore)]
        public String[] Users  { get; set; }

        [JsonProperty(PropertyName = "description", NullValueHandling=NullValueHandling.Ignore)]
        public String Description { get; set; }

        [JsonProperty(PropertyName = "point", NullValueHandling=NullValueHandling.Ignore)]
        public int? EstimatePoint { get; set; }

        [JsonProperty(PropertyName = "duration", NullValueHandling=NullValueHandling.Ignore)]
        public String duration { get; set; }

        [JsonProperty(PropertyName = "startdate", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty(PropertyName = "enddate", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
    }
}