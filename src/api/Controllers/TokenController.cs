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
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetAccessToken(string code)
    {
        var result = await service.GetAccessTokenAsync(code).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetAccessTokenFromRefreshTokenAsync(string refreshToken)
    {
        var result = await service.GetAccessTokenFromRefreshTokenAsync(refreshToken).ConfigureAwait(false);
        return Ok(result);
    }
}