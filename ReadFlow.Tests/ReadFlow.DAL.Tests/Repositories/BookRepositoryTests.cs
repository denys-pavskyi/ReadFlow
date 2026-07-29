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
[Category("Books")]
public class BookRepositoryTests
{
    private InMemoryDbFixture _fixture = null!;
    private AppDbContext _context = null!;
    private IBookRepository _repository = null!;
    private BookBuilder _bookBuilder = null!;
    private List<Genre> _genres = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new InMemoryDbFixture();
        _context = _fixture.CreateContext();
        _repository = new BookRepository(_context);

        _genres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Fiction", Description = "Fictional works", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Fantasy", Description = "Fantasy works", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Science Fiction", Description = "Sci-fi works", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _context.Genres.AddRange(_genres);
        _context.SaveChanges();

        _bookBuilder = new BookBuilder(_genres);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _fixture.Dispose();
    }

    [Test]
    public async Task GetByIdAsync_ExistingBook_ReturnsBook()
    {
        var book = _bookBuilder.WithTitle("Test Book").Build();
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(book.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(book.Id);
        result.Title.Should().Be("Test Book");
    }

    [Test]
    public async Task GetByIdAsync_NonExistentBook_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task GetBookWithGenresAsync_ExistingBookWithGenres_ReturnsBookWithGenres()
    {
        var book = _bookBuilder
            .WithTitle("Fantasy Book")
            .WithAuthor("J.R.R. Tolkien")
            .WithGenres(_genres[0], _genres[1])
            .Build();

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _repository.GetBookWithGenresAsync(book.Id);

        result.Should().NotBeNull();
        result!.BookGenres.Should().HaveCount(2);
        result.BookGenres.Select(bg => bg.Genre.Name).Should().Contain(new[] { "Fiction", "Fantasy" });
    }

    [Test]
    public async Task GetBookWithGenresAsync_NonExistentBook_ReturnsNull()
    {
        var result = await _repository.GetBookWithGenresAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task AddAsync_ValidBook_AddsBookToDatabase()
    {
        var book = _bookBuilder
            .WithTitle("New Book")
            .WithAuthor("New Author")
            .WithISBN("1234567890")
            .Build();

        await _repository.AddAsync(book);
        await _context.SaveChangesAsync();

        var savedBook = await _context.Books.FindAsync(book.Id);
        savedBook.Should().NotBeNull();
        savedBook!.Title.Should().Be("New Book");
        savedBook.Author.Should().Be("New Author");
        savedBook.ISBN.Should().Be("1234567890");
    }

    [Test]
    public async Task UpdateAsync_ExistingBook_UpdatesBook()
    {
        var book = _bookBuilder.WithTitle("Original Title").Build();
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        book.Title = "Updated Title";
        book.PageCount = 500;
        await _repository.UpdateAsync(book);

        var updatedBook = await _context.Books.FindAsync(book.Id);
        updatedBook.Should().NotBeNull();
        updatedBook!.Title.Should().Be("Updated Title");
        updatedBook.PageCount.Should().Be(500);
    }

    [Test]
    public async Task DeleteAsync_ExistingBook_RemovesBook()
    {
        var book = _bookBuilder.Build();
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(book.Id);

        var deletedBook = await _context.Books.FindAsync(book.Id);
        deletedBook.Should().BeNull();
    }

    [Test]
    public async Task ExistsAsync_ExistingBook_ReturnsTrue()
    {
        var book = _bookBuilder.Build();
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var exists = await _repository.ExistsAsync(book.Id);

        exists.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_NonExistentBook_ReturnsFalse()
    {
        var exists = await _repository.ExistsAsync(Guid.NewGuid());

        exists.Should().BeFalse();
    }

    [Test]
    public async Task GetAllAsync_MultipleBooksExist_ReturnsAllBooks()
    {
        var books = _bookBuilder.Build(5);
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(5);
    }
}
