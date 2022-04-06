using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class LogWorkController : ControllerBase
{
    private readonly ILogWorkService service;

    public LogWorkController(ILogWorkService service)
    {
        this.service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<LogWork>), 200)]
    public async Task<IActionResult> SearchAsync([FromBody]LogWorkSearchParameter parameter)
    {
        var result = await service.SearchAsync(parameter).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("search-by-global-view")]
    [ProducesResponseType(typeof(IEnumerable<LogWork>), 200)]
    public async Task<IActionResult> SearchByGlobalViewAsync([FromBody]LogWorkSearchParameter parameter)
    {
        var result = await service.SearchByGlobalViewAsync(parameter).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(LogWork), 201)]
    public async Task<IActionResult> CreateAsync([FromBody]LogWorkSaveParameter parameter)
    {
        parameter.Action = "additemlog";
        var result = await service.CreateAsync(parameter).ConfigureAwait(false);
        return new CreatedResult(string.Empty, result);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateAsync([FromBody]LogWorkSaveParameter parameter)
    {
        parameter.Action = "updateitemlog";
        await service.UpdateAsync(parameter).ConfigureAwait(false);
        return NoContent();
    }
}