using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Application;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class ZohoScheduleController : ControllerBase
{
    private readonly ZohoHostedService service;

    public ZohoScheduleController(ZohoHostedService service)
    {
        this.service = service;
    }

    [HttpPost("start")]
    [ProducesResponseType(typeof(ScheduleSetting), 200)]
    public async Task<IActionResult> StartAsync([FromBody] ScheduleSetting parameter)
    {
        var result = await service.StartAsync(parameter).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("stop")]
    [ProducesResponseType(typeof(ScheduleSetting), 200)]
    public async Task<IActionResult> StopAsync()
    {
        var result = await service.StopAsync().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("status")]
    [ProducesResponseType(typeof(ScheduleSetting), 200)]
    public async Task<IActionResult> StatusAsync()
    {
        var result = await service.GetStatusAsync().ConfigureAwait(false);
        return Ok(result);
    }
}