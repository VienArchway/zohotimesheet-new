using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class LogWorkSaveParameter
    {
        public string LogTimeId { get; set; }

        public string ProjId { get; set; }

        public string SprintId { get; set; }

        public string ItemId { get; set; }

        [JsonProperty(PropertyName = "action", NullValueHandling=NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "duration", NullValueHandling=NullValueHandling.Ignore)]
        public string Duration { get; set; }

        [JsonProperty(PropertyName = "date", NullValueHandling=NullValueHandling.Ignore)]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "users", NullValueHandling=NullValueHandling.Ignore)]
        public string Users { get; set; }

        [JsonProperty(PropertyName = "isbillable", NullValueHandling=NullValueHandling.Ignore)]
        public int Isbillable { get; set; }
    }
}