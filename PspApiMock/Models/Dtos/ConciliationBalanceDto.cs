namespace PspApiMock.Models.Dtos;

public class ConciliationBalanceDto
{
    public HashSet<PaymentFileDto> DatabaseToFile { get; set; }

    public HashSet<PaymentFileDto> FileToDatabase { get; set; }

    public HashSet<PaymentIdDto> DifferentStatus { get; set; }
}

public class PaymentFileDto
{
    public required Guid Id { get; set; }

    public required string Status { get; set; }

    public DateTime Date { get; set; }
}

public class PaymentIdDto
{
    public required Guid Id { get; set; }
}