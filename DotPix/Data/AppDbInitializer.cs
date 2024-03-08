using DotPix.Data.Seed;
using DotPix.Models;

namespace DotPix.Data;

public static class AppDbInitializer
{
    public static WebApplication Seed(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            var account = context.PaymentProviderAccount.FirstOrDefault();
            if (account == null)
            {
                Console.WriteLine("Initiating database seeding process...");
                SeedDataUtils.SaveSeed(context);
                Console.WriteLine("Database seeding process completed successfully!");
            }
            else
                Console.WriteLine("Database has already been seeded.");

            return app;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during seeding: {e.Message}");
            throw new Exception(message: "Seed Error! See log for details.");
        }
    }
}