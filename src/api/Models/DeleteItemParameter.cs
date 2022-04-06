using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class DeleteTaskItemParameter
    {
        public string TaskItemId { get; set; }

        public string ProjId { get; set; }

        public string SprintId { get; set; }
    }
}