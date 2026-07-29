using Bogus;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Tests.Builders;

public class BookBuilder
{
    private readonly Faker<Book> _faker;
    private readonly List<Genre> _availableGenres;

    public BookBuilder(List<Genre> availableGenres)
    {
        _availableGenres = availableGenres;

        _faker = new Faker<Book>()
            .RuleFor(b => b.Id, _ => Guid.NewGuid())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5).TrimEnd('.'))
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.ISBN, f => f.Random.Replace("##########"))
            .RuleFor(b => b.Description, f => f.Lorem.Paragraphs(2))
            .RuleFor(b => b.PublishedDate, f => DateTime.SpecifyKind(f.Date.Past(50), DateTimeKind.Utc))
            .RuleFor(b => b.PageCount, f => f.Random.Int(100, 1500))
            .RuleFor(b => b.CoverImageUrl, f => f.Internet.Avatar())
            .RuleFor(b => b.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(b => b.UpdatedAt, _ => DateTime.UtcNow);
    }

    public Book Build() => _faker.Generate();

    public List<Book> Build(int count) => _faker.Generate(count);

    public BookBuilder WithTitle(string title)
    {
        _faker.RuleFor(b => b.Title, title);
        return this;
    }

    public BookBuilder WithAuthor(string author)
    {
        _faker.RuleFor(b => b.Author, author);
        return this;
    }

    public BookBuilder WithISBN(string isbn)
    {
        _faker.RuleFor(b => b.ISBN, isbn);
        return this;
    }

    public BookBuilder WithDescription(string description)
    {
        _faker.RuleFor(b => b.Description, description);
        return this;
    }

    public BookBuilder PublishedYearsAgo(int years)
    {
        _faker.RuleFor(b => b.PublishedDate, _ => DateTime.SpecifyKind(DateTime.UtcNow.AddYears(-years), DateTimeKind.Utc));
        return this;
    }

    public BookBuilder WithPageCount(int pageCount)
    {
        _faker.RuleFor(b => b.PageCount, pageCount);
        return this;
    }

    public BookBuilder WithRandomGenres(int count = 3)
    {
        _faker.RuleFor(b => b.BookGenres, _ =>
        {
            var selectedGenres = new Faker().PickRandom(_availableGenres, Math.Min(count, _availableGenres.Count)).ToList();
            return selectedGenres.Select(g => new BookGenre
            {
                GenreId = g.Id
            }).ToList();
        });
        return this;
    }

    public BookBuilder WithGenres(params Genre[] genres)
    {
        _faker.RuleFor(b => b.BookGenres, _ =>
            genres.Select(g => new BookGenre
            {
                GenreId = g.Id
            }).ToList());
        return this;
    }
}
