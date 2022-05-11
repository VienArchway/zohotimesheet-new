using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class ZohoTokenController : ControllerBase
{
    private readonly IZohoTokenService service;

    private readonly ITeamClient teamClient;

    public ZohoTokenController(IZohoTokenService service, ITeamClient teamClient)
    {
        this.service = service;
        this.teamClient = teamClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> GetAccessToken([FromQuery] string code)
    {
        var result = await service.GetAccessTokenAsync(code).ConfigureAwait(false);
        return Ok(result);
    }
    
    [HttpGet("refresh-access-token")]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> GetAccessTokenFromRefreshTokenAsync([FromQuery]string refreshToken)
    {
        var result = await service.GetAccessTokenFromRefreshTokenAsync(refreshToken).ConfigureAwait(false);
        return Ok(result.AccessToken);
    }

    [Authorize]
    [HttpGet("revoke")]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromQuery]string token)
    {
        await service.RevokeRefreshTokenAsync(token).ConfigureAwait(false);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("verify-token")]
    public async Task<IActionResult> Get()
    {
        var result = await teamClient.SearchAsync().ConfigureAwait(false);
        if (string.IsNullOrEmpty(result))
        {
            return Unauthorized();
        }
        return Ok(result);
    }
}