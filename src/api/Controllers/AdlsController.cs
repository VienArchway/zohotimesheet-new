using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;
using System.Security.Claims;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class AdlsController : ControllerBase
{
    private readonly IAdlsService service;

    public AdlsController(IAdlsService service)
    {
        this.service = service;
    }

    [HttpGet("get-from-adls")]
    [ProducesResponseType(typeof(IEnumerable<LogWork>), 200)]
    public async Task<IActionResult> GetFromAdlsAsync()
    {
        var result = await service.GetFromAdlsAsync().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("delete-from-adls")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteFromAdlsAsync([FromBody] string[] AdlsIds)
    {
        await service.DeleteFromAdlsAsync(AdlsIds).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPost("transfer")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> TransferAdlsAsync([FromBody]LogWorkSearchParameter parameter)
    {
        // var userName = User.FindFirst(ClaimTypes.Name).Value; // why we need this????
        var result = await service.TransferAdlsAsync(parameter, null).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("restore-from-adls")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RestoreFromAdlsAsync()
    {
        await service.RestoreFromAdlsAsync().ConfigureAwait(false);
        return Ok();
    }
}