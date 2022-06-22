using System.Security.Claims;
using System.Text.Encodings.Web;
using api.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace api.Services.Security;

public class ZohoAuthenticationHandler : AuthenticationHandler<ZohoAuthenticationSchema>
{
    private readonly ITeamClient teamClient;

    private String archwayTeamId;
    
    public ZohoAuthenticationHandler(
        ITeamClient teamClient,
        IOptionsMonitor<ZohoAuthenticationSchema> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        this.teamClient = teamClient;
        this.archwayTeamId = configuration.GetValue<string>("Zoho:TeamId");
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        
        // handle if controller have not required authorized annotation
        if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var fetchTeam = teamClient.GetTeamSettingAsync("signout").GetAwaiter().GetResult();
        if (fetchTeam == null)
        {
            return Task.FromResult(AuthenticateResult.Fail($"Wrong access token"));
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, fetchTeam?["firstName"].ToString()),
            new Claim(ClaimTypes.Name, "Name"),
        };

        var id = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(id);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}