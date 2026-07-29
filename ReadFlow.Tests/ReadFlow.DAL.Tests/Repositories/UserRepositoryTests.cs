using FluentAssertions;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Implementations;
using ReadFlow.DAL.Repositories.Interfaces;
using ReadFlow.DAL.Tests.Builders;
using ReadFlow.DAL.Tests.Fixtures;

namespace ReadFlow.DAL.Tests.Repositories;

[TestFixture]
[Category("Integration")]
[Category("DAL")]
[Category("Analytics")]
public class UserRepositoryTests
{
    private DatabaseFixture _fixture = null!;
    private AppDbContext _context = null!;
    private IUserRepository _repository = null!;
    private UserBuilder _userBuilder = null!;
    private BookBuilder _bookBuilder = null!;
    private BookRatingBuilder _ratingBuilder = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _fixture = new DatabaseFixture();
        _fixture.GlobalSetup().Wait();
    }

    [SetUp]
    public void Setup()
    {
        _context = new AppDbContext(_fixture.Options);
        _repository = new UserRepository(_context);

        _userBuilder = new UserBuilder();
        _bookBuilder = new BookBuilder(_fixture.SeedGenres);
        _ratingBuilder = new BookRatingBuilder();

        _fixture.ResetDatabaseAsync().Wait();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.GlobalTeardown().Wait();
    }

    [Test]
    public async Task GetByUsernameAsync_ExistingUser_ReturnsUser()
    {
        var user = _userBuilder.WithUsername("testuser").WithEmail("test@example.com").Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByUsernameAsync("testuser");

        result.Should().NotBeNull();
        result!.Username.Should().Be("testuser");
        result.Email.Should().Be("test@example.com");
    }

    [Test]
    public async Task GetByUsernameAsync_NonExistentUser_ReturnsNull()
    {
        var result = await _repository.GetByUsernameAsync("nonexistent");

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByEmailAsync_ExistingUser_ReturnsUser()
    {
        var user = _userBuilder.WithEmail("test@example.com").Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByEmailAsync("test@example.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }

    [Test]
    public async Task GetByEmailAsync_NonExistentUser_ReturnsNull()
    {
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        result.Should().BeNull();
    }

    [Test]
    public async Task GetUserRatingsAsync_UserWithRatings_ReturnsRatingsWithBooks()
    {
        var user = _userBuilder.Build();
        var books = _bookBuilder.WithRandomGenres(2).Build(3);

        await _context.Users.AddAsync(user);
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        var ratings = new List<BookRating>
        {
            _ratingBuilder.ForUser(user.Id).ForBook(books[0].Id).WithRating(8).Build(),
            _ratingBuilder.ForUser(user.Id).ForBook(books[1].Id).WithRating(9).Build(),
            _ratingBuilder.ForUser(user.Id).ForBook(books[2].Id).WithRating(7).Build()
        };

        await _context.BookRatings.AddRangeAsync(ratings);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _repository.GetUserRatingsAsync(user.Id);

        result.Should().HaveCount(3);
        result.Select(r => r.Rating).Should().Contain(new[] { 8, 9, 7 });
        result.All(r => r.Book != null).Should().BeTrue();
        result.All(r => r.Book.BookGenres.Any()).Should().BeTrue();
    }

    [Test]
    public async Task GetUserRatingsAsync_UserWithNoRatings_ReturnsEmptyList()
    {
        var user = _userBuilder.Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetUserRatingsAsync(user.Id);

        result.Should().BeEmpty();
    }

    [Test]
    [Category("Slow")]
    public async Task GetPlatformAverageRatingAsync_MultipleRatingsExist_ReturnsCorrectAverage()
    {
        var users = _userBuilder.Build(3);
        var books = _bookBuilder.Build(5);

        await _context.Users.AddRangeAsync(users);
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        var ratings = new List<BookRating>
        {
            _ratingBuilder.ForUser(users[0].Id).ForBook(books[0].Id).WithRating(8).Build(),
            _ratingBuilder.ForUser(users[1].Id).ForBook(books[1].Id).WithRating(6).Build(),
            _ratingBuilder.ForUser(users[2].Id).ForBook(books[2].Id).WithRating(10).Build(),
            _ratingBuilder.ForUser(users[0].Id).ForBook(books[3].Id).WithRating(7).Build(),
            _ratingBuilder.ForUser(users[1].Id).ForBook(books[4].Id).WithRating(9).Build()
        };

        await _context.BookRatings.AddRangeAsync(ratings);
        await _context.SaveChangesAsync();

        var result = await _repository.GetPlatformAverageRatingAsync();

        result.Should().Be(8.0m);
    }

    [Test]
    public async Task GetUserGenreRatingsAsync_UserRatedBooksInMultipleGenres_ReturnsGenreRatings()
    {
        var user = _userBuilder.Build();
        var fictionGenre = _fixture.SeedGenres.First(g => g.Name == "Fiction");
        var sciFiGenre = _fixture.SeedGenres.First(g => g.Name == "Science Fiction");

        var books = new List<Book>
        {
            _bookBuilder.WithTitle("Fiction Book 1").WithGenres(fictionGenre).Build(),
            _bookBuilder.WithTitle("Fiction Book 2").WithGenres(fictionGenre).Build(),
            _bookBuilder.WithTitle("SciFi Book").WithGenres(sciFiGenre).Build()
        };

        await _context.Users.AddAsync(user);
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        var ratings = new List<BookRating>
        {
            _ratingBuilder.ForUser(user.Id).ForBook(books[0].Id).WithRating(8).Build(),
            _ratingBuilder.ForUser(user.Id).ForBook(books[1].Id).WithRating(6).Build(),
            _ratingBuilder.ForUser(user.Id).ForBook(books[2].Id).WithRating(9).Build()
        };

        await _context.BookRatings.AddRangeAsync(ratings);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _repository.GetUserGenreRatingsAsync(user.Id);

        result.Should().HaveCount(3);
        result.Where(r => r.GenreName == "Fiction").Should().HaveCount(2);
        result.Where(r => r.GenreName == "Science Fiction").Should().ContainSingle();
        result.Where(r => r.GenreName == "Fiction").Select(r => r.Rating).Should().Contain(new[] { 8, 6 });
    }
}
