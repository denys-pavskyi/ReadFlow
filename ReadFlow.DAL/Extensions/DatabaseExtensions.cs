using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Data.Seeders;

namespace ReadFlow.DAL.Extensions;

public static class DatabaseExtensions
{
    public static async Task SeedDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();

        try
        {
            await context.Database.MigrateAsync();

            logger.LogInformation("Starting database seeding...");
            var seeder = new DatabaseSeeder(context);
            await seeder.SeedAllAsync();
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding database");
            throw;
        }
    }
}
