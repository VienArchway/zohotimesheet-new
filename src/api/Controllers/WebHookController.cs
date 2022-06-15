using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Application.Interfaces;
using api.Models;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class WebHookController : ControllerBase
{    
    private readonly IWebHookService service;

    public WebHookController(IWebHookService service)
    {
        this.service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WebHook>), 200)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await service.GetAllAsync().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("update-status")]
    [ProducesResponseType(typeof(WebHook), 201)]
    public async Task<IActionResult> UpdateStatusAsync([FromBody]WebHookStatusUpdateParameter parameter)
    {
        parameter.Action = "updatestatus";
        await service.UpdateStatusAsync(parameter).ConfigureAwait(false);
        return NoContent();
    }
}