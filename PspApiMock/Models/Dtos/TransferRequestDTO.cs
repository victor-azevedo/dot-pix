namespace PspApiMock.Models.Dtos;

public class TransferStatus
{
    public required OriginDTO Origin { get; set; }
    public required DestinyDTO Destiny { get; set; }
    public required int Amount { get; set; }
    public string? Description { get; set; }
    public required Guid PaymentId { get; set; }
}

public class OriginDTO
{
    public required UserDTO User { get; set; }
    public required AccountDTO Account { get; set; }
}

public class UserDTO
{
    public required string Name { get; set; }
    public required string MaskedCpf { get; set; }
}

public class AccountDTO
{
    public required string Number { get; set; }
    public required string Agency { get; set; }
    public required string BankName { get; set; }
}

public class DestinyDTO
{
    public required KeyDTO Key { get; set; }
}

public class KeyDTO
{
    public required string Value { get; set; }
    public required string Type { get; set; } // CPF, Email, Phone ou Random
}