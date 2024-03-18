namespace PspApiMock.Models.Dtos;

public class TransferStatusDTO
{
    public required Guid PaymentId { get; set; }
    public required string Status { get; set; }
}