using System.Text.Json;
using DotPix.Models;
using static System.IO.File;

namespace DotPix.Data.Seed;

public class SeedHandler(AppDbContext context)
{
    private const string SeedDataJsonPathDefault = "./Data/Seed/seed.json";

    public void EnsureDatabaseIsPopulated()
    {
        var someUserOrNull = context.User.FirstOrDefault();
        var somePaymentProviderOrNull = context.PaymentProvider.FirstOrDefault();

        if (IsDatabaseAlreadyPopulated(someUserOrNull, somePaymentProviderOrNull))
        {
            Console.WriteLine("Database already populated.");
            return;
        }

        Console.WriteLine("Initiating database seeding process...");

        var seedData = GetSeedData();

        if (someUserOrNull == null)
            PopulateUserData(seedData.Users);

        if (somePaymentProviderOrNull == null)
            PopulatePaymentProviderData(seedData.PaymentProviders);

        Console.WriteLine("Database has been seeded.");
    }

    private bool IsDatabaseAlreadyPopulated(User? someUser, PaymentProvider? somePaymentProvider)
    {
        return someUser != null && somePaymentProvider != null;
    }

    private void PopulateUserData(List<SeedDataDto.SeedDataUser> seedDataUsers)
    {
        var users = seedDataUsers.Select(seedDataUser => new User(name: seedDataUser.Name, cpf: seedDataUser.Cpf))
            .ToList();
        context.User.AddRange(users);
        context.SaveChanges();
    }


    private void PopulatePaymentProviderData(List<SeedDataDto.SeedDataPaymentProvider> seedDataPaymentProviders)
    {
        ClearPaymentProviderToken();
        seedDataPaymentProviders.ForEach(seedDataPaymentProvider =>
        {
            var paymentProvider = new PaymentProvider(seedDataPaymentProvider.Name);
            context.PaymentProvider.Add(paymentProvider);
            context.PaymentProviderToken.Add(new PaymentProviderToken(seedDataPaymentProvider.Token)
            {
                PaymentProvider = paymentProvider
            });
        });
        context.SaveChanges();
    }

    private void ClearPaymentProviderToken()
    {
        context.PaymentProviderToken.RemoveRange(context.PaymentProviderToken);
    }

    private SeedDataDto GetSeedData(string seedDataJsonPath = SeedDataJsonPathDefault)
    {
        try
        {
            var seedJson = ReadAllText(seedDataJsonPath);
            var seedData = JsonSerializer.Deserialize<SeedDataDto>(seedJson);
            if (seedData == null)
                throw new JsonException();
            return seedData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(
                $"Seed error. Please check the log for details.");
        }
    }
}