using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class ItemSearchParameter
    {
        public IEnumerable<string>? itemIds { get; set; }

        public IEnumerable<string>? itemNos { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public DateTime? StartDateTo { get; set; }

        public IEnumerable<int>? SprintTypeIds { get; set; }

        public IEnumerable<string>? CompletedOn { get; set; }

        public IEnumerable<string>? Assignees { get; set; }

        public int StatusId { get; set; }
    }
}