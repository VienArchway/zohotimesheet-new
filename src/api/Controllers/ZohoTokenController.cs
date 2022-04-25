using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]

public class ZohoTokenController : ControllerBase
{
    private readonly IZohoTokenService service;

    public ZohoTokenController(IZohoTokenService service)
    {
        this.service = service;
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
        return Ok(result);
    }

    [HttpGet("revoke")]
    [ProducesResponseType(typeof(Token), 200)]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromQuery]string token)
    {
        await service.RevokeRefreshTokenAsync(token).ConfigureAwait(false);
        return Ok();
    }
}