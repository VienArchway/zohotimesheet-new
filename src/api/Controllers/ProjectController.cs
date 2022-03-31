using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

// [Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService service;

    public ProjectController(IProjectService service)
    {
        this.service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
    public async Task<IActionResult> Search()
    {
        var result = await service.SearchAsync().ConfigureAwait(false);
        return Ok(result);
    }
}