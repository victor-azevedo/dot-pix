namespace DotPixApi.Data.Seed;

public class SeedDataDto
{
    public List<SeedDataUser> Users { get; set; }
    public List<SeedDataPaymentProvider> PaymentProviders { get; set; }

    public class SeedDataUser
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
    }

    public class SeedDataPaymentProvider
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }
    }
}