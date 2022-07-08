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
    }
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        
        // handle if controller have not required authorized annotation
        if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Name"),
        };

        var id = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(id);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        var fetchTeam = teamClient.FetchTeamsAsync().GetAwaiter().GetResult();
        return Task.FromResult(fetchTeam == null ? AuthenticateResult.Fail($"Wrong access token") : AuthenticateResult.Success(ticket));
    }
}