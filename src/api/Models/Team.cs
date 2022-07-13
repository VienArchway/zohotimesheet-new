using Newtonsoft.Json;

namespace api.Models
{
    public class Team
    {
        [JsonProperty(PropertyName = "teamName")]
        public String TeamName { get; set; }

        [JsonProperty(PropertyName = "isShowTeam")]
        public bool SsShowTeam { get; set; }

        [JsonProperty(PropertyName = "orgName")]
        public String OrgName { get; set; }

        [JsonProperty(PropertyName = "portalOwner")]
        public String PortalOwner { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "zsoid")]
        public String Zsoid { get; set; }
    }
}