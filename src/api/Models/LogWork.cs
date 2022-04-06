using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class LogWork
    {
        [JsonProperty(PropertyName = "id")]
        public string LogTimeId { get; set; }

        [JsonProperty(PropertyName = "sprintId")]
        public string SprintId { get; set; }

        [JsonProperty(PropertyName = "itemNo")]
        public string ItemNo { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "tLogId")]
        public string TLogId { get; set; }

        [JsonProperty(PropertyName = "Owner")]
        public string Owner { get; set; }

        [JsonProperty(PropertyName = "OwnerName")]
        public string OwnerName { get; set; }

        [JsonProperty(PropertyName = "logDate")]
        public string LogDate { get; set; }

        [JsonProperty(PropertyName = "logTime")]
        public float? LogTime { get; set; }

        [JsonProperty(PropertyName = "itemId")]
        public string ItemId { get; set; }

        [JsonProperty(PropertyName = "billableType")]
        public int BillableType { get; set; }

        [JsonProperty(PropertyName = "approveType")]
        public int ApproveType { get; set; }

        [JsonProperty(PropertyName = "tClockId")]
        public string TClockId { get; set; }

        [JsonProperty(PropertyName = "addedBy")]
        public string AddedBy { get; set; }

        [JsonProperty(PropertyName = "logNotes")]
        public string LogNotes { get; set; }

        [JsonProperty(PropertyName = "approvedBy")]
        public string ApprovedBy { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId")]
        public string ProjItemTypeId { get; set; }

        public string ProjName { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public string ProjNo { get; set; }

        [JsonProperty(PropertyName = "projId")]
        public string ProjId { get; set; }
    }
}