using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class UpdateItemStatusParameter
    {
        public String TaskItemId { get; set; }

        public String ProjId { get; set; }

        public String SprintId { get; set; }

        [JsonProperty(PropertyName = "statusid", NullValueHandling=NullValueHandling.Ignore)]
        public String StatusId { get; set; }

        [JsonProperty(PropertyName = "action", NullValueHandling=NullValueHandling.Ignore)]
        public String Action
        {
            get
            {
                return "updatestatus";
            }
        }
    }
}