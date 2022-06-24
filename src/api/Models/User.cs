using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string? UserId { get; set; }

        [JsonProperty(PropertyName = "zsUserId")]
        public string? ZSUserId { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string? DisplayName { get; set; }

        public string? FirstName { get; set; }

        [JsonProperty(PropertyName = "emailId")]
        public string? EmailId { get; set; }

        [JsonProperty(PropertyName = "isConfirmed")]
        public bool IsConfirmed { get; set; }

        [JsonProperty(PropertyName = "userRole")]
        public int UserRole { get; set; }
    }
}