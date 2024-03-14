
using DotPixApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPixApi.Controllers;

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