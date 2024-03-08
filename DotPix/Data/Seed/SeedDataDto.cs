using DotPix.Models;

namespace DotPix.Data.Seed;

public class SeedDataDto
{
    public SeedDataUser User { get; set; }
    public SeedDataPaymentProvider PaymentProvider { get; set; }
    public SeedDataPaymentProviderAccount PaymentProviderAccount { get; set; }

    public class SeedDataUser
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
    }

    public class SeedDataPaymentProvider
    {
        public string Name { get; set; }
        public string Token { get; set; }
    }

    public class SeedDataPaymentProviderAccount
    {
        public string Account { get; set; }
        public string Agency { get; set; }
    }
}