using Microsoft.AspNetCore.Authentication;

namespace api.Services.Security;

public class ZohoAuthenticationSchema : AuthenticationSchemeOptions
{
    public const String DefaultSchemeName = "zoho";
}