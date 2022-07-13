using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class LogWork
    {
        [JsonProperty(PropertyName = "id")]
        public String LogTimeId { get; set; }

        [JsonProperty(PropertyName = "sprintId")]
        public String SprintId { get; set; }

        [JsonProperty(PropertyName = "itemNo")]
        public String ItemNo { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public String ItemName { get; set; }

        [JsonProperty(PropertyName = "tLogId")]
        public String TLogId { get; set; }

        [JsonProperty(PropertyName = "Owner")]
        public String Owner { get; set; }

        [JsonProperty(PropertyName = "OwnerName")]
        public String OwnerName { get; set; }

        [JsonProperty(PropertyName = "logDate")]
        public String LogDate { get; set; }

        [JsonProperty(PropertyName = "logTime")]
        public float? LogTime { get; set; }

        [JsonProperty(PropertyName = "itemId")]
        public String ItemId { get; set; }

        [JsonProperty(PropertyName = "billableType")]
        public int BillableType { get; set; }

        [JsonProperty(PropertyName = "approveType")]
        public int ApproveType { get; set; }

        [JsonProperty(PropertyName = "tClockId")]
        public String TClockId { get; set; }

        [JsonProperty(PropertyName = "addedBy")]
        public String AddedBy { get; set; }

        [JsonProperty(PropertyName = "logNotes")]
        public String LogNotes { get; set; }

        [JsonProperty(PropertyName = "approvedBy")]
        public String ApprovedBy { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId")]
        public String ProjItemTypeId { get; set; }

        public String ProjName { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public String ProjNo { get; set; }

        [JsonProperty(PropertyName = "projId")]
        public String ProjId { get; set; }
    }
}