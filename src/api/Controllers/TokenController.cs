using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]

public class TokenController : ControllerBase
{
    private readonly ITokenService service;

    public TokenController(ITokenService service)
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

    // [HttpGet]
    // [ProducesResponseType(typeof(Token), 200)]
    // public async Task<IActionResult> RevokeRefreshTokenAsync(string token)
    // {
    //     await service.RevokeRefreshTokenAsync(token).ConfigureAwait(false);
    //     return Ok();
    // }
}