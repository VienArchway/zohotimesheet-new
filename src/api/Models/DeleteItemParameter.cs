using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class DeleteItemParameter
    {
        public String ItemId { get; set; }

        public String ProjId { get; set; }

        public String SprintId { get; set; }
    }
}