using System.Text.Json.Serialization;

namespace DotPixConciliationWorker.Models.Dtos;

public class ConciliationDto
{
    [JsonPropertyName("databaseToFile")]
    public List<PaymentFileDto> DatabaseToFile { get; set; }

    [JsonPropertyName("fileToDatabase")]
    public List<PaymentFileDto> FileToDatabase { get; set; }

    [JsonPropertyName("differentStatus")]
    public List<PaymentIdDto> DifferentStatus { get; set; }
}

public class PaymentIdDto
{
    public required Guid Id { get; set; }
}