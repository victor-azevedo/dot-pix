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
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> Processing payment" +
        $"\n\tID: '{dto.PaymentId}'" +
        $"\n\t- Origin: CPF '{dto.Origin.User.MaskedCpf}' to:" +
        $"\n\t- Destiny: Pix key '{dto.Destiny.Key.Type}' - '{dto.Destiny.Key.Value}'");
    var timeToWait = GenerateResponseWaitingTime();
    Console.WriteLine($"This operation will return in {timeToWait} ms");
    Thread.Sleep(timeToWait);

    return Results.Ok();
});

app.MapPatch("/payments/pix", (TransferStatusDTO dto) =>
{
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> Payment processed" +
        $"\n\tID: '{dto.PaymentId}' STATUS: '{dto.Status}'");
    return Results.Accepted();
});

app.MapPost("/conciliation", (ConciliationBalanceDto dto) =>
{
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> Conciliation balance received" +
        $"\n\tCount of 'databaseToFile': '{dto.DatabaseToFile.Count}'" +
        $"\n\tCount of 'fileToDatabase': '{dto.FileToDatabase.Count}'" +
        $"\n\tCount of 'differentStatus': '{dto.DifferentStatus.Count}'");
    return Results.Accepted();
    return Results.Accepted();
});

app.MapPost("/conciliation", (ConciliationBalanceDto dto) =>
{
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> Conciliation balance received" +
        $"\n\tCount of 'databaseToFile': '{dto.DatabaseToFile.Count}'" +
        $"\n\tCount of 'fileToDatabase': '{dto.FileToDatabase.Count}'" +
        $"\n\tCount of 'differentStatus': '{dto.DifferentStatus.Count}'");
    return Results.Accepted();
});

app.MapGet("/health", () =>
{
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> PSP Mock healthy");
    return Results.Accepted();
});

app.MapGet("/health", () =>
{
    Console.WriteLine(
        $"\n***************************" +
        $"\n--> PSP Mock healthy");
    return Results.Accepted();
});

app.UseHttpsRedirection();

app.Run();

return;

static int GenerateResponseWaitingTime()
{
    const int SLOW_RESPONSE_PERCENTAGE = 30;

    const int SLOW_RESPONSE_MIN_WAITING_TIME_SEC = 10 * 1000;
    const int SLOW_RESPONSE_MAX_WAITING_TIME_SEC = 500 * 1000;

    const int NORMAL_RESPONSE_MIN_WAITING_TIME_MILLI_SEC = 300;
    const int NORMAL_RESPONSE_MAX_WAITING_TIME_MILLI_SEC = 500;

    var random = new Random();
    var percentageChoice = random.Next(1, 101);

    if (percentageChoice <= SLOW_RESPONSE_PERCENTAGE)
        return random.Next(SLOW_RESPONSE_MIN_WAITING_TIME_SEC, SLOW_RESPONSE_MAX_WAITING_TIME_SEC);

    return random.Next(NORMAL_RESPONSE_MIN_WAITING_TIME_MILLI_SEC, NORMAL_RESPONSE_MAX_WAITING_TIME_MILLI_SEC);
}