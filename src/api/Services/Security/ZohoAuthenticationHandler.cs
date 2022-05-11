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
        ISystemClock clock)
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
        
        // handle request only for refresh token
        if (Request.Headers.ContainsKey(Options.RefreshTokenHeaderName)
            || Request.Headers.ContainsKey(Options.IsRedirectUri))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        // check include access token
        if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required header for token: {Options.TokenHeaderName}"));
        }
        
        var accessToken = Request.Headers[Options.TokenHeaderName].ToString();

        if (accessToken is "null" or "" or "undefined")
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required header for token: {Options.TokenHeaderName}"));
        }
        
        var fetchTeam = teamClient.SearchAsync().GetAwaiter().GetResult();
        
        if (string.IsNullOrEmpty(fetchTeam) || !fetchTeam.Equals("success"))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Wrong access token: {Options.TokenHeaderName}"));
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "NameIdentifier"),
            new Claim(ClaimTypes.Name, "Name"),
        };

        var id = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(id);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}