using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class ReadingGoalSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.ReadingGoals.AnyAsync())
        {
            return;
        }

        var users = await context.Users.ToListAsync();
        var goals = new List<ReadingGoal>();

        var usersWithGoals = users.OrderBy(_ => Guid.NewGuid()).Take(users.Count / 2);

        var goalFaker = new Faker<ReadingGoal>()
            .RuleFor(g => g.TargetBookCount, f => f.Random.Int(10, 100))
            .RuleFor(g => g.Description, f => f.Lorem.Sentence());

        foreach (var user in usersWithGoals)
        {
            var goal2025 = goalFaker.Generate();
            goal2025.UserId = user.Id;
            goal2025.Year = 2025;
            goals.Add(goal2025);

            var goal2026 = goalFaker.Generate();
            goal2026.UserId = user.Id;
            goal2026.Year = 2026;
            goals.Add(goal2026);
        }

        await context.ReadingGoals.AddRangeAsync(goals);
        await context.SaveChangesAsync();
    }
}
