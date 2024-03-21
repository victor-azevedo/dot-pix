namespace DotPixConciliationWorker.Models.Dtos;

public class InConciliationQueueDto
{
    public required int PaymentProviderId { get; set; }
    public required DateTime Date { get; set; }
    public required string File { get; set; }
    public required string Postback { get; set; }
}