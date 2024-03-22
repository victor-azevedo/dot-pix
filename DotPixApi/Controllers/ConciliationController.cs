using System.Net;
using DotPixApi.Models.Dtos;
using DotPixApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPixApi.Controllers;

[ApiController]
[Route("conciliation")]
public class ConciliationController(ConciliationService conciliationService) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] InPostConciliationDto inPostConciliationDto)
    {
        conciliationService.SendConciliationToWorker(inPostConciliationDto);

        return StatusCode(HttpStatusCode.Accepted.GetHashCode());
    }
}