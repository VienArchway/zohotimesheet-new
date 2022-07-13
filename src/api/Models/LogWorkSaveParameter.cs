using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class LogWorkSaveParameter
    {
        public String LogTimeId { get; set; }

        public String ProjId { get; set; }

        public String SprintId { get; set; }

        public String ItemId { get; set; }

        [JsonProperty(PropertyName = "action", NullValueHandling=NullValueHandling.Ignore)]
        public String Action { get; set; }

        [JsonProperty(PropertyName = "duration", NullValueHandling=NullValueHandling.Ignore)]
        public String Duration { get; set; }

        [JsonProperty(PropertyName = "date", NullValueHandling=NullValueHandling.Ignore)]
        public String Date { get; set; }

        [JsonProperty(PropertyName = "users", NullValueHandling=NullValueHandling.Ignore)]
        public String Users { get; set; }

        [JsonProperty(PropertyName = "isbillable", NullValueHandling=NullValueHandling.Ignore)]
        public int Isbillable { get; set; }
    }
}