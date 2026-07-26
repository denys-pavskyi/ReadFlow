using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Enums;

namespace ReadFlow.DAL.Data.Seeders;

public class UserSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword("Password123!"))
            .RuleFor(u => u.DisplayName, f => f.Name.FullName())
            .RuleFor(u => u.Bio, f => f.Lorem.Paragraph())
            .RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.Role, f => f.PickRandom<UserRole>());

        var users = userFaker.Generate(75);

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}
