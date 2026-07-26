using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Data.Seeders;

namespace ReadFlow.DAL.Extensions;

public static class DatabaseExtensions
{
    public static async Task SeedDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Check if already seeded (check if any users exist)
        if (await context.Users.AnyAsync())
        {
            return; // Already seeded
        }

        var seeder = new DatabaseSeeder(context);
        await seeder.SeedAllAsync();
    }
}
