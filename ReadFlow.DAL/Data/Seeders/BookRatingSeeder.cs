using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class BookRatingSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.BookRatings.AnyAsync())
        {
            return;
        }

        var users = await context.Users.ToListAsync();
        var books = await context.Books.ToListAsync();
        var ratings = new List<BookRating>();

        var random = new Random();

        foreach (var user in users)
        {
            var ratingCount = random.Next(10, 51);
            var selectedBooks = books.OrderBy(_ => Guid.NewGuid()).Take(ratingCount);

            foreach (var book in selectedBooks)
            {
                var ratingFaker = new Faker<BookRating>()
                    .RuleFor(r => r.UserId, _ => user.Id)
                    .RuleFor(r => r.BookId, _ => book.Id)
                    .RuleFor(r => r.Rating, f => f.Random.Int(1, 10))
                    .RuleFor(r => r.CreatedAt, f => DateTime.SpecifyKind(
                        f.Date.Between(DateTime.UtcNow.AddYears(-2), DateTime.UtcNow),
                        DateTimeKind.Utc));

                ratings.Add(ratingFaker.Generate());
            }
        }

        await context.BookRatings.AddRangeAsync(ratings);
        await context.SaveChangesAsync();
    }
}
