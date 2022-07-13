using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public String UserId { get; set; }

        [JsonProperty(PropertyName = "zsUserId")]
        public String ZSUserId { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public String DisplayName { get; set; }

        public String FirstName { get; set; }

        [JsonProperty(PropertyName = "emailId")]
        public String EmailId { get; set; }

        [JsonProperty(PropertyName = "isConfirmed")]
        public bool IsConfirmed { get; set; }

        [JsonProperty(PropertyName = "userRole")]
        public int UserRole { get; set; }
        
    }
}