using Microsoft.AspNetCore.Authentication;

namespace api.Services.Security;

public class ZohoAuthenticationSchema : AuthenticationSchemeOptions
{
    public const string DefaultSchemeName = "zoho";
    public string TokenHeaderName { get; set; } = "Zoho-Verify-Token";
    public string ClientIdHeaderName { get; } = "Zoho-Client-Id";
}