using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Tests.Fixtures;

[SetUpFixture]
public class DatabaseFixture
{
    private SqliteConnection? _connection;
    private DbContextOptions<AppDbContext>? _options;
    private static readonly List<Genre> _seedGenres = new()
    {
        new Genre { Id = Guid.NewGuid(), Name = "Fiction", Description = "Fictional works" },
        new Genre { Id = Guid.NewGuid(), Name = "Science Fiction", Description = "Science fiction works" },
        new Genre { Id = Guid.NewGuid(), Name = "Fantasy", Description = "Fantasy works" },
        new Genre { Id = Guid.NewGuid(), Name = "Mystery", Description = "Mystery works" },
        new Genre { Id = Guid.NewGuid(), Name = "Thriller", Description = "Thriller works" }
    };

    public DbContextOptions<AppDbContext> Options => _options ?? throw new InvalidOperationException("Database not initialized");
    public List<Genre> SeedGenres => _seedGenres;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        await using var context = new AppDbContext(_options);
        await context.Database.EnsureCreatedAsync();

        context.Genres.AddRange(_seedGenres);
        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task ResetDatabaseAsync()
    {
        await using var context = new AppDbContext(Options);

        context.BookRatings.RemoveRange(context.BookRatings);
        context.Reviews.RemoveRange(context.Reviews);
        context.Comments.RemoveRange(context.Comments);
        context.CommentVotes.RemoveRange(context.CommentVotes);
        context.CollectionBooks.RemoveRange(context.CollectionBooks);
        context.Collections.RemoveRange(context.Collections);
        context.BookGenres.RemoveRange(context.BookGenres);
        context.Books.RemoveRange(context.Books);
        context.PostLikes.RemoveRange(context.PostLikes);
        context.Posts.RemoveRange(context.Posts);
        context.UserFollows.RemoveRange(context.UserFollows);
        context.ReadingGoals.RemoveRange(context.ReadingGoals);
        context.Users.RemoveRange(context.Users);

        await context.SaveChangesAsync();
    }
}
