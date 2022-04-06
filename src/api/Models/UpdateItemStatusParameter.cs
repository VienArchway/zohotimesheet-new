using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class UpdateTaskItemStatusParameter
    {
        public string TaskItemId { get; set; }

        public string ProjId { get; set; }

        public string SprintId { get; set; }

        [JsonProperty(PropertyName = "statusid", NullValueHandling=NullValueHandling.Ignore)]
        public string StatusId { get; set; }

        [JsonProperty(PropertyName = "action", NullValueHandling=NullValueHandling.Ignore)]
        public string Action
        {
            get
            {
                return "updatestatus";
            }
        }
    }
}