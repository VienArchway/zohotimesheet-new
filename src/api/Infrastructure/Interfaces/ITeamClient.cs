using api.Models;
using Newtonsoft.Json.Linq;

namespace api.Infrastructure.Interfaces
{
    public interface ITeamClient
    {
        Task<JObject?> GetTeamSettingAsync(string action = null, string accessToken = null);
    }
}
