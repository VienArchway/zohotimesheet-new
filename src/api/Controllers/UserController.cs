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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{userid}")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetZSUserIdIdByUserIdAsync(string userid)
    {
        var result = await service.GetZSUserIdIdByUserIdAsync(userid).ConfigureAwait(false);
        return Ok(result);
    }
}