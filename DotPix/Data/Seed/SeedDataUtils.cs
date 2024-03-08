using System.Text.Json;
using DotPix.Models;

namespace DotPix.Data.Seed;

public static class SeedDataUtils
{
    private static string ReadFile(string filePath)
    {
        try
        {
            using StreamReader reader = new StreamReader(filePath);
            var content = reader.ReadToEnd();

            return content;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during read file: {e.Message}");
            throw new Exception(message: "Read file Error! See log for details.");
        }
    }

    private static List<SeedDataDto> LoadSeedJson()
    {
        const string seedDataJsonFile = "./Data/Seed/seed.json";

        var json = ReadFile(seedDataJsonFile);

        try
        {
            var seedDataList = JsonSerializer.Deserialize<List<SeedDataDto>>(json);
            return seedDataList ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during load seed json: {e.Message}");
            throw new Exception(message: "Load json Error! See log for details.");
        }
    }

    public static void SaveSeed(AppDbContext context)
    {
        var seedData = LoadSeedJson();

        foreach (var accountData in seedData)
        {
            var user = new User(name: accountData.User.Name, cpf: accountData.User.Cpf);
            context.User.Add(user);

            var pp = context.PaymentProvider
                .FirstOrDefault(p => p.Name == accountData.PaymentProvider.Name);

            PaymentProvider paymentProvider;
            if (pp != null)
                paymentProvider = pp;
            else
            {
                paymentProvider = new PaymentProvider(name: accountData.PaymentProvider.Name);

                var paymentProviderToken = new PaymentProviderToken(token: accountData.PaymentProvider.Token)
                {
                    PaymentProvider = paymentProvider
                };

                context.PaymentProvider.Add(paymentProvider);
                context.PaymentProviderToken.Add(paymentProviderToken);
            }

            var paymentProviderAccount = new PaymentProviderAccount(
                account: accountData.PaymentProviderAccount.Account,
                agency: accountData.PaymentProviderAccount.Agency)
            {
                User = user,
                PaymentProvider = paymentProvider
            };
            context.PaymentProviderAccount.Add(paymentProviderAccount);

            context.SaveChanges();
        }
    }
}