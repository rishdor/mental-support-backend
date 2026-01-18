using Microsoft.AspNetCore.Mvc;

namespace mental_support.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
        return Ok(new { status = "Healthy" });
    }
}