using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class EpicController : ControllerBase
{
    private readonly IEpicService service;

    public EpicController(IEpicService service)
    {
        this.service = service;
    }

    [HttpGet("{projId}")]
    [ProducesResponseType(typeof(IEnumerable<Epic>), 200)]
    public async Task<IActionResult> Search(String projId)
    {
        var result = await service.SearchAsync(projId).ConfigureAwait(false);
        return Ok(result);
    }
}