using System.Security.Claims;
using System.Text.Encodings.Web;
using api.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace api.Services.Security;

public class ZohoAuthenticationHandler : AuthenticationHandler<ZohoAuthenticationSchema>
{
    private readonly IConfiguration config;

    private readonly ITeamClient teamClient;
    
    public ZohoAuthenticationHandler(ITeamClient teamClient, IOptionsMonitor<ZohoAuthenticationSchema> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config)
        : base(options, logger, encoder, clock)
    {
        this.config = config;
        this.teamClient = teamClient;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // check include clientId
        if (!Request.Headers.ContainsKey(Options.ClientIdHeaderName))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required clientId: {Options.ClientIdHeaderName}"));
        }
        
        if (!Request.Headers[Options.ClientIdHeaderName].ToString().Equals(config["Zoho:ClientId"]))
        {
            // check match clientId
            return Task.FromResult(AuthenticateResult.Fail($"ClientId is not match: {Options.ClientIdHeaderName}"));
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
        
        // TODO: check expired
        var fetchTeam = teamClient.SearchAsync().GetAwaiter().GetResult();
        
        if (!fetchTeam.Any())
        {
            return Task.FromResult(AuthenticateResult.Fail($"Wrong access token: {Options.TokenHeaderName}"));
        }
        
        // TODO: save refresh_token to somewhere

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