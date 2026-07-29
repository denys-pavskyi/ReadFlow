using Bogus;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Tests.Builders;

public class BookRatingBuilder
{
    private readonly Faker<BookRating> _faker;

    public BookRatingBuilder()
    {
        _faker = new Faker<BookRating>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 10))
            .RuleFor(r => r.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(r => r.UpdatedAt, _ => DateTime.UtcNow);
    }

    public BookRating Build() => _faker.Generate();

    public List<BookRating> Build(int count) => _faker.Generate(count);

    public BookRatingBuilder WithRating(int rating)
    {
        _faker.RuleFor(r => r.Rating, rating);
        return this;
    }

    public BookRatingBuilder ForUser(Guid userId)
    {
        _faker.RuleFor(r => r.UserId, userId);
        return this;
    }

    public BookRatingBuilder ForBook(Guid bookId)
    {
        _faker.RuleFor(r => r.BookId, bookId);
        return this;
    }

    public BookRatingBuilder CreatedAt(DateTime createdAt)
    {
        _faker.RuleFor(r => r.CreatedAt, DateTime.SpecifyKind(createdAt, DateTimeKind.Utc));
        _faker.RuleFor(r => r.UpdatedAt, DateTime.SpecifyKind(createdAt, DateTimeKind.Utc));
        return this;
    }

    public BookRatingBuilder WithinLastMonth()
    {
        var daysAgo = new Faker().Random.Int(0, 30);
        var date = DateTime.UtcNow.AddDays(-daysAgo);
        _faker.RuleFor(r => r.CreatedAt, date);
        _faker.RuleFor(r => r.UpdatedAt, date);
        return this;
    }

    public BookRatingBuilder WithinLastYear()
    {
        var daysAgo = new Faker().Random.Int(0, 365);
        var date = DateTime.UtcNow.AddDays(-daysAgo);
        _faker.RuleFor(r => r.CreatedAt, date);
        _faker.RuleFor(r => r.UpdatedAt, date);
        return this;
    }

    public BookRatingBuilder YearsAgo(int years)
    {
        var date = DateTime.UtcNow.AddYears(-years);
        _faker.RuleFor(r => r.CreatedAt, date);
        _faker.RuleFor(r => r.UpdatedAt, date);
        return this;
    }
}
