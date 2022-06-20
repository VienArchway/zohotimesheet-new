using api.Models;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;

namespace api.Services.Security;

public class ZohoKeyVaultHandler
{
    private SecretClient SecretClient { get; }
    
    private KeyVaultSecret RefreshTokenSecret { get; set; }

    private readonly IConfiguration configuration;

    private readonly System.Net.Http.HttpClient client;

    public ZohoKeyVaultHandler(IConfiguration configuration, KeyVaultSecret refreshTokenSecret, System.Net.Http.HttpClient client)
    {
        this.configuration = configuration;
        this.client = client;
        RefreshTokenSecret = refreshTokenSecret;
        SecretClient = new SecretClient(
            new Uri("https://zohotoken.vault.azure.net/"),
            new DefaultAzureCredential());
    }

    public async Task<string> FetchSecretRefreshToken(string zohoRefreshToken)
    {
        var zohoUser = await GetZohoUserInfo();
        Environment.SetEnvironmentVariable("displayName", zohoUser.DisplayName);
        try
        {
            RefreshTokenSecret = await SecretClient.GetSecretAsync(
                $"refreshtoken-{Environment.GetEnvironmentVariable("displayName")}", null, CancellationToken.None);
        }
        catch (RequestFailedException e)
        {
            Console.WriteLine(e);
            if (e.Status == 404)
            {
                await SetSecretRefreshToken(zohoRefreshToken);
            }
            else
            {
                throw;
            }
        }
        finally
        {
            if (string.IsNullOrEmpty(RefreshTokenSecret.Value))
            {
                await SetSecretRefreshToken(zohoRefreshToken);
            }
        }

        return RefreshTokenSecret.Value;
    }

    private async Task SetSecretRefreshToken(string secretValue)
    {
        var result = await SecretClient.SetSecretAsync(
            $"refreshtoken-{Environment.GetEnvironmentVariable("displayName")}", secretValue, CancellationToken.None);
        RefreshTokenSecret = result.Value;
    }
    
    private async Task<ZohoUser> GetZohoUserInfo()
    {
        var resUser = await client.GetAsync(configuration["Zoho:UserHost"]);
        var userInfo = await resUser.Content.ReadAsStringAsync().ConfigureAwait(false);
        var user = JsonConvert.DeserializeObject<ZohoUser>(userInfo);

        return user ?? throw new InvalidOperationException();
    }
}