using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class TaskItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "projId")]
        public string ProjId { get; set; }

        [JsonProperty(PropertyName = "isParent")]
        public bool IsParent { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "itemNo")]
        public string ItemNo { get; set; }

        [JsonProperty(PropertyName = "ownerId")]
        public List<string> OwnerId { get; set; }

        public IEnumerable<User> Users { get; set; }

        [JsonProperty(PropertyName = "points")]
        public string Points { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "sprintType")]
        public int SprintType { get; set; }

        [JsonProperty(PropertyName = "sprintName")]
        public string SprintName { get; set; }

        [JsonProperty(PropertyName = "sprintNo")]
        public string SprintNo { get; set; }

        [JsonProperty(PropertyName = "projPriorityId")]
        public string ProjPriorityId { get; set; }

        [JsonProperty(PropertyName = "projName")]
        public string ProjName { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public string ProjNo { get; set; }

        [JsonProperty(PropertyName = "completedDate")]
        public string CompletedDate { get; set; }

        [JsonProperty(PropertyName = "sprintId")]
        public string SprintId { get; set; }

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }

        [JsonProperty(PropertyName = "statusId")]
        public string StatusId { get; set; }

        public string StatusName { get; set; }

        [JsonProperty(PropertyName = "epicId")]
        public string EpicId { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId")]
        public string ProjItemTypeId { get; set; }

        public string ProjItemName { get; set; }

        [JsonProperty(PropertyName = "projectId")]
        public string ProjectId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        public IEnumerable<string> SubItemIds { get; set; }
    }
}