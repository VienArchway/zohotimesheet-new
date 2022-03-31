using Newtonsoft.Json;

namespace api.Models
{
    public class Team
    {
        [JsonProperty(PropertyName = "teamName")]
        public string? TeamName { get; set; }

        [JsonProperty(PropertyName = "isShowTeam")]
        public bool SsShowTeam { get; set; }

        [JsonProperty(PropertyName = "orgName")]
        public string? OrgName { get; set; }

        [JsonProperty(PropertyName = "portalOwner")]
        public string? PortalOwner { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "zsoid")]
        public string? Zsoid { get; set; }
    }
}