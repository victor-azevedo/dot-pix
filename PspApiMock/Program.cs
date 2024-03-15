using PspApiMock.Models.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/payments/pix", (TransferStatus dto) =>
{
    Console.WriteLine($"Processing payment from {dto.Origin.User.CPF} to {dto.Destiny.Key.Value}");
    var timeToWait = GenerateResponseWaitingTime();
    Console.WriteLine($"This operation will return in {timeToWait} ms");
    Thread.Sleep(timeToWait);

    return Results.Ok();
});

app.MapPatch("/payments/pix", (TransferStatusDTO dto) =>
{
    Console.WriteLine($"Processing payment status id {dto.Id} to {dto.Status}");
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();

return;

static int GenerateResponseWaitingTime()
{
    const int slowResponsePercentage = 10;

    const int slowResponseMinWaitingTimeSec = 3 * 1000;
    const int slowResponseMaxWaitingTimeSec = 10 * 1000;

    const int normalResponseMinWaitingTimeMilliSec = 3 * 100;
    const int normalResponseMaxWaitingTimeMilliSec = 5 * 100;

    var random = new Random();
    var percentageChoice = random.Next(1, 101);

    if (percentageChoice <= slowResponsePercentage)
        return random.Next(slowResponseMinWaitingTimeSec, slowResponseMaxWaitingTimeSec);

    return random.Next(normalResponseMinWaitingTimeMilliSec, normalResponseMaxWaitingTimeMilliSec);
}