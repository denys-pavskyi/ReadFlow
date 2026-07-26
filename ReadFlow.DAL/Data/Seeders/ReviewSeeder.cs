using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class ReviewSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.Reviews.AnyAsync())
        {
            return;
        }

        var ratings = await context.BookRatings.ToListAsync();
        var reviews = new List<Review>();

        var ratingsWithReviews = ratings.OrderBy(_ => Guid.NewGuid()).Take((int)(ratings.Count * 0.3));

        var reviewFaker = new Faker<Review>()
            .RuleFor(r => r.Content, f => f.Rant.Review());

        foreach (var rating in ratingsWithReviews)
        {
            var review = reviewFaker.Generate();
            review.UserId = rating.UserId;
            review.BookId = rating.BookId;
            review.CreatedAt = rating.CreatedAt;

            reviews.Add(review);
        }

        await context.Reviews.AddRangeAsync(reviews);
        await context.SaveChangesAsync();
    }
}
