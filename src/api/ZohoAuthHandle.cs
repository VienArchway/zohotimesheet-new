using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace api;

public class ZohoAuthHandle : AuthenticationHandler<ZohoSetting>
{
    private readonly IConfiguration _config;
    
    public ZohoAuthHandle(IOptionsMonitor<ZohoSetting> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config)
        : base(options, logger, encoder, clock)
    {
        _config = config;
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
        
        const string teamPath = "/teams/";
        var fetchTeam = ZohoHttpClient.RunAsync(accessToken, teamPath, _config).GetAwaiter().GetResult();
        
        if (!fetchTeam.Equals("success"))
        {
            return Task.FromResult(AuthenticateResult.Fail($"Wrong access token: {Options.TokenHeaderName}"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, fetchTeam),
            new Claim(ClaimTypes.Name, fetchTeam),
        };

        var id = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(id);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}