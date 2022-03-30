using System.Net.Http.Headers;
using static System.String;

namespace api;

public record TeamClient(object Portals, string Status);

public static class ZohoHttpClient
{
    private static readonly HttpClient Client = new();
    private static string _status = Empty;
    
    private static async Task<TeamClient?> FetchTeamClient(string path)
    {
        var teamClient = new TeamClient("", "");
        var response = await Client.GetAsync(Client.BaseAddress + path);
        if (response.IsSuccessStatusCode)
        {
            teamClient = await response.Content.ReadFromJsonAsync<TeamClient>();
        }
        return teamClient;
    }

    public static async Task<string> RunAsync(string apiKey, string path, IConfiguration config)
    {
        if (Client.BaseAddress == null)
        {
            Client.BaseAddress = new Uri(config["Zoho:BaseUrl"]);
            Client.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {apiKey}");
        }
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Add("User-Agent", "Archway");
        Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            var fetchTeamClient = await FetchTeamClient(path);
            Console.WriteLine(fetchTeamClient.Portals);
            _status = fetchTeamClient.Status;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return _status;
    }
}