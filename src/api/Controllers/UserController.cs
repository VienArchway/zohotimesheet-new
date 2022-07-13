using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }

    [HttpGet("info")]
    [ProducesResponseType(typeof(String), 200)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await service.GetCurrentUser().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(String), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync().ConfigureAwait(false);
        return Ok(result);
    }
}