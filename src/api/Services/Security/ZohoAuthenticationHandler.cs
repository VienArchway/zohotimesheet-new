using System.Security.Claims;
using System.Text.Encodings.Web;
using api.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace api.Services.Security;

public class ZohoAuthenticationHandler : AuthenticationHandler<ZohoAuthenticationSchema>
{
    private readonly IConfiguration _config;

    private readonly ITeamClient teamClient;
    
    public ZohoAuthenticationHandler(ITeamClient teamClient, IOptionsMonitor<ZohoAuthenticationSchema> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config)
        : base(options, logger, encoder, clock)
    {
        _config = config;
        this.teamClient = teamClient;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // check include clientId
        if (!Request.Headers.ContainsKey(Options.ClientIdHeaderName))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required clientId: {Options.ClientIdHeaderName}"));
        }
        
        if (!Request.Headers[Options.ClientIdHeaderName].ToString().Equals(_config["Zoho:Client_Id"]))
        {
            // check match clientId
            return Task.FromResult(AuthenticateResult.Fail($"ClientId is not match: {Options.ClientIdHeaderName}"));
        }
        
        // check include access token
        if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required header for token: {Options.TokenHeaderName}"));
        }

        var accessToken = Request.Headers[Options.TokenHeaderName];

        if (accessToken.Equals("null") || accessToken.Equals("undefined"))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Required header for token: {Options.TokenHeaderName}"));
        }
        
        // TODO: check expired
        var fetchTeam = this.teamClient.SearchAsync().GetAwaiter().GetResult();
        
        if (!fetchTeam.Any())
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