using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    private readonly ISprintService service;

    public SprintController(ISprintService service)
    {
        this.service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Sprint>), 200)]
    public async Task<IActionResult> SearchAsync([FromQuery] SprintSearchParameter parameter)
    {
        var result = await service.SearchAsync(parameter).ConfigureAwait(false);
        return Ok(result);
    }
}