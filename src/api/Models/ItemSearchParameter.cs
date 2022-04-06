using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class TaskItemSearchParameter
    {
        public IEnumerable<string>? TaskItemIds { get; set; }

        public IEnumerable<string>? TaskItemNos { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public DateTime? StartDateTo { get; set; }

        public IEnumerable<int>? SprintTypeIds { get; set; }

        public IEnumerable<string>? CompletedOn { get; set; }

        public IEnumerable<string>? Assignees { get; set; }

        public int StatusId { get; set; }
    }
}