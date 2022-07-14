using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;
using System.Security.Claims;

namespace api.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class ZohoTokenController : ControllerBase
{
    private readonly IZohoTokenService service;


    public ZohoTokenController(IZohoTokenService service, ITeamClient teamClient)
    {
        this.service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> GetAccessToken([FromQuery] String code)
    {
        var result = await service.GetAccessTokenAsync(code).ConfigureAwait(false);
        
        var cookieOptions = new CookieOptions
        {
            // Secure = true,
            // HttpOnly = true,
            SameSite = SameSiteMode.Unspecified
        };
        Response.Cookies.Append("accessToken", result.AccessToken ?? String.Empty, cookieOptions);
        
        return Ok(result);
    }
    
    [HttpGet("get-access-by-refresh-token")]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> GetAccessTokenByRefreshTokenAsync([FromQuery] String firstName, [FromQuery] String zsUserId)
    {
        var result = await service.GetAccessTokenByRefreshTokenAsync(firstName, zsUserId).ConfigureAwait(false);
        var cookieOptions = new CookieOptions
        {
            // Secure = true,
            // HttpOnly = true,
            SameSite = SameSiteMode.Unspecified
        };
        Response.Cookies.Append("accessToken", result.AccessToken ?? String.Empty, cookieOptions);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("revoke")]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> RevokeRefreshTokenAsync()
    {
        await service.RevokeRefreshTokenAsync().ConfigureAwait(false);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("logout")]
    public Task<IActionResult> Logout()
    {
        var cookieOptions = new CookieOptions
        {
            SameSite = SameSiteMode.Unspecified
        };
        Response.Cookies.Delete("accessToken", cookieOptions);

        return Task.FromResult<IActionResult>(Ok());
    }
}