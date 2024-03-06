
using DotPix.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPix.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController(HealthService healthService):ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(healthService.GetHealthMessage());
    }
}