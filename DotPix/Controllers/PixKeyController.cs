using DotPix.Models.Dtos;
using DotPix.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPix.Controllers;

[ApiController]
[Route("keys")]
public class PixKeyController(PixKeyService pixKeyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingCreatePixKeyDto incomingCreatePixKeyDto)
    {
        await pixKeyService.Create(incomingCreatePixKeyDto);

        return CreatedAtAction(null, null, incomingCreatePixKeyDto.Key);
    }
}