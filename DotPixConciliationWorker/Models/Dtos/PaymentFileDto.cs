using System.Text.Json.Serialization;

namespace DotPixConciliationWorker.Models.Dtos;

public class PaymentFileDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("status")]
    public required PaymentStatus Status { get; set; }
}