using System.Text.Json.Serialization;

namespace DotPixConciliationWorker.Models.Dtos;

public class PaymentFileDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    public DateTime Date { get; set; }
}