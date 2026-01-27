using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Data;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("/health")]
    public async Task<IActionResult> GetHealth()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            return Ok(new { status = "Healthy" });
        }
        catch
        {
            return StatusCode(503, new { status = "Unhealthy", error = "Database connection failed" });
        }
    }
}