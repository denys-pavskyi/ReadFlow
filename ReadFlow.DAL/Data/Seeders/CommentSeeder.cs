using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class CommentSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.Comments.AnyAsync())
        {
            return;
        }

        var users = await context.Users.ToListAsync();
        var books = await context.Books.ToListAsync();
        var comments = new List<Comment>();
        var random = new Random();
        var commentFaker = new Faker<Comment>()
            .RuleFor(c => c.Content, f => f.Lorem.Sentences(3))
            .RuleFor(c => c.UserId, f => f.PickRandom(users).Id)
            .RuleFor(c => c.BookId, f => f.PickRandom(books).Id)
            .RuleFor(c => c.ParentCommentId, _ => null)
            .RuleFor(c => c.CreatedAt, f => DateTime.SpecifyKind(
                f.Date.Between(DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow),
                DateTimeKind.Utc));

        var topLevelComments = commentFaker.Generate(150);
        await context.Comments.AddRangeAsync(topLevelComments);
        await context.SaveChangesAsync();

        var commentsWithReplies = topLevelComments.OrderBy(_ => Guid.NewGuid()).Take(45);

        foreach (var parentComment in commentsWithReplies)
        {
            var replyCount = random.Next(1, 4);

            var replyFaker = new Faker<Comment>()
                .RuleFor(c => c.Content, f => f.Lorem.Sentences(2))
                .RuleFor(c => c.UserId, f => f.PickRandom(users).Id)
                .RuleFor(c => c.BookId, _ => parentComment.BookId)
                .RuleFor(c => c.ParentCommentId, _ => parentComment.Id)
                .RuleFor(c => c.CreatedAt, f => f.Date.Between(
                    parentComment.CreatedAt,
                    DateTime.UtcNow));

            comments.AddRange(replyFaker.Generate(replyCount));
        }

        await context.Comments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
    }
}
