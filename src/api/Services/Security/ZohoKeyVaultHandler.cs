using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace api.Services.Security;

public class ZohoKeyVaultHandler
{
    private SecretClient SecretClient { get; }
    
    private KeyVaultSecret RefreshTokenSecret { get; set; }

    private readonly IConfiguration configuration;

    public ZohoKeyVaultHandler(IConfiguration configuration, KeyVaultSecret refreshTokenSecret)
    {
        this.configuration = configuration;
        RefreshTokenSecret = refreshTokenSecret;
        SecretClient = new SecretClient(
            new Uri("https://zohotoken.vault.azure.net/"),
            new DefaultAzureCredential());
    }

    public async Task<string> FetchSecretRefreshToken(string zohoRefreshToken)
    {
        try
        {
            RefreshTokenSecret = await SecretClient.GetSecretAsync(
                $"refreshtoken-{configuration["userName"]}", null, CancellationToken.None);
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
            $"refreshtoken-{configuration["userName"]}", secretValue, CancellationToken.None);
        RefreshTokenSecret = result.Value;
    }
}