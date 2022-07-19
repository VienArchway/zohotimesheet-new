using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class TaskItem
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "projId")]
        public String ProjId { get; set; }

        [JsonProperty(PropertyName = "isParent")]
        public bool IsParent { get; set; }

        [JsonProperty(PropertyName = "parentItem")]
        public String ParentItemId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public String EndDate { get; set; }

        [JsonProperty(PropertyName = "itemNo")]
        public String ItemNo { get; set; }

        [JsonProperty(PropertyName = "ownerId")]
        public List<String> OwnerId { get; set; }

        public IEnumerable<User> Users { get; set; }

        [JsonProperty(PropertyName = "points")]
        public String Points { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public String Duration { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public String ItemName { get; set; }

        [JsonProperty(PropertyName = "sprintType")]
        public int SprintType { get; set; }

        [JsonProperty(PropertyName = "sprintName")]
        public String SprintName { get; set; }

        [JsonProperty(PropertyName = "sprintNo")]
        public String SprintNo { get; set; }

        [JsonProperty(PropertyName = "projPriorityId")]
        public String ProjPriorityId { get; set; }

        [JsonProperty(PropertyName = "projName")]
        public String ProjName { get; set; }

        [JsonProperty(PropertyName = "projNo")]
        public String ProjNo { get; set; }

        [JsonProperty(PropertyName = "completedDate")]
        public String CompletedDate { get; set; }

        [JsonProperty(PropertyName = "sprintId")]
        public String SprintId { get; set; }

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }

        [JsonProperty(PropertyName = "statusId")]
        public String StatusId { get; set; }

        public String StatusName { get; set; }

        [JsonProperty(PropertyName = "epicId")]
        public String EpicId { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public String CreatedBy { get; set; }

        [JsonProperty(PropertyName = "projItemTypeId")]
        public String ProjItemTypeId { get; set; }

        public String ProjItemTypeName { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public String StartDate { get; set; }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }

        public IEnumerable<String> SubItemIds { get; set; }
    }
}