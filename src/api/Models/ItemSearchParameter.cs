using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class TaskItemSearchParameter
    {
        public IEnumerable<String> TaskItemIds { get; set; }

        public IEnumerable<String> TaskItemNos { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public DateTime? StartDateTo { get; set; }

        public IEnumerable<int> SprintTypeIds { get; set; }

        public IEnumerable<String> CompletedOn { get; set; }

        public IEnumerable<String> Assignees { get; set; }

        public int StatusId { get; set; }
    }
}