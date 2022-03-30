using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    public IActionResult Get(string? msg)
    {
        return Ok(msg != null ? $"Hello {msg}" : "Hello default message");
    }
}