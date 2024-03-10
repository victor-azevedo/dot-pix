using DotPix.Data.Seed;

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

            var seedHandler = new SeedHandler(context);
            seedHandler.EnsureDatabaseIsPopulated();

            return app;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during seeding: {e}");
            throw new Exception(message: "Seed Error! See log for details.");
        }
    }
}