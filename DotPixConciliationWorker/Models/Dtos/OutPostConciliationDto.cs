using System.Text.Json.Serialization;

namespace DotPixConciliationWorker.Models.Dtos;

public class OutPostConciliationDto
{
    [JsonPropertyName("databaseToFile")]
    public HashSet<PaymentFileDto> DatabaseToFile { get; set; }

    [JsonPropertyName("fileToDatabase")]
    public HashSet<PaymentFileDto> FileToDatabase { get; set; }

    [JsonPropertyName("differentStatus")]
    public HashSet<PaymentIdDto> DifferentStatus { get; set; }
}

public class PaymentIdDto
{
    public required Guid Id { get; set; }
}