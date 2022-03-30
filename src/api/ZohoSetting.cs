using Microsoft.AspNetCore.Authentication;

namespace api;

public class ZohoSetting : AuthenticationSchemeOptions
{
    public const string DefaultSchemeName = "zoho";
    public string TokenHeaderName { get; set; } = "Zoho-Verify-Token";
    public string ClientIdHeaderName { get; } = "Zoho-Client-Id";
}