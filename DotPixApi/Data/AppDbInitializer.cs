using DotPixApi.Data.Seed;

namespace DotPixApi.Data;

public static class AppDbInitializer
{
    private const string SeedDevJsonPath = "../Database/seedDev.json";
    private const string SeedTestJsonPath = "../Database/seedTest.json";

    public static WebApplication Seed(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            var seedJsonPath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
                ? SeedTestJsonPath
                : SeedDevJsonPath;

            var seedHandler = new SeedHandler(context, seedJsonPath);
            seedHandler.EnsureDatabaseIsPopulated();

            return app;
        }
        catch
            (Exception e)
        {
            Console.WriteLine($"Error during seeding: {e}");
            throw new Exception(message: "Seed Error! See log for details.");
        }
    }
}