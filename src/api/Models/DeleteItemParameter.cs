using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class DeleteItemParameter
    {
        public string ItemId { get; set; }

        public string ProjId { get; set; }

        public string SprintId { get; set; }
    }
}