using DotPixApi.Models.Dtos;
using DotPixApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotPixApi.Controllers;

[ApiController]
[Route("payments")]
public class PaymentController(PaymentService paymentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomingCreatePaymentDto incomingCreatePaymentDto)
    {
        var response = await paymentService.Create(incomingCreatePaymentDto);

        return CreatedAtAction(null, null, response);
    }
}