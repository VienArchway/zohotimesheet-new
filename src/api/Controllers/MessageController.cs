using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    public IActionResult Get(string? msg)
    {
        return Ok(msg != null ? $"Hello {msg}" : "Hello default message");
    }
}