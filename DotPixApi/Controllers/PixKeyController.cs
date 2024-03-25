using DotPixApi.Models.Dtos;
using DotPixApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPixApi.Controllers;

[ApiController]
[Route("keys")]
public class PixKeyController(PixKeyService pixKeyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InPostKeysDto inPostKeysDto)
    {
        await pixKeyService.Create(inPostKeysDto);

        return CreatedAtAction(null, null, inPostKeysDto.Key);
    }

    [HttpGet("{type}/{value}")]
    public async Task<IActionResult> FindKey(InPixKeyDto inPixKey)
    {
        var response = await pixKeyService.FindByTypeAndValueIncludeAccountOrThrow(inPixKey);

        return Ok(response);
    }
}