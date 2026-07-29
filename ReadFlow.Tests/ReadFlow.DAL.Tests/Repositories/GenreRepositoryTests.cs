using FluentAssertions;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Implementations;
using ReadFlow.DAL.Repositories.Interfaces;
using ReadFlow.DAL.Tests.Fixtures;

namespace ReadFlow.DAL.Tests.Repositories;

[TestFixture]
[Category("Integration")]
[Category("DAL")]
[Category("Genres")]
public class GenreRepositoryTests
{
    private DatabaseFixture _fixture = null!;
    private AppDbContext _context = null!;
    private IGenreRepository _repository = null!;

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
        _repository = new GenreRepository(_context);
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
    public async Task GetAllAsync_SeedGenresExist_ReturnsSeedGenres()
    {
        var result = await _repository.GetAllAsync();

        result.Should().HaveCountGreaterThanOrEqualTo(_fixture.SeedGenres.Count);
        result.Select(g => g.Name).Should().Contain(_fixture.SeedGenres.Select(g => g.Name));
    }

    [Test]
    public async Task GetByIdAsync_ExistingGenre_ReturnsGenre()
    {
        var genreId = _fixture.SeedGenres.First().Id;

        var result = await _repository.GetByIdAsync(genreId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(genreId);
    }

    [Test]
    public async Task GetByIdAsync_NonExistentGenre_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task AddAsync_NewGenre_AddsGenreToDatabase()
    {
        var newGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Horror",
            Description = "Horror and suspense",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(newGenre);
        await _context.SaveChangesAsync();

        var savedGenre = await _context.Genres.FindAsync(newGenre.Id);
        savedGenre.Should().NotBeNull();
        savedGenre!.Name.Should().Be("Horror");
    }

    [Test]
    public async Task UpdateAsync_ExistingGenre_UpdatesGenre()
    {
        var genre = _fixture.SeedGenres.First();
        var existingGenre = await _context.Genres.FindAsync(genre.Id);
        existingGenre!.Description = "Updated description";

        await _repository.UpdateAsync(existingGenre);
        await _context.SaveChangesAsync();

        var updatedGenre = await _context.Genres.FindAsync(genre.Id);
        updatedGenre!.Description.Should().Be("Updated description");
    }

    [Test]
    public async Task ExistsAsync_ExistingGenre_ReturnsTrue()
    {
        var genreId = _fixture.SeedGenres.First().Id;

        var exists = await _repository.ExistsAsync(genreId);

        exists.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_NonExistentGenre_ReturnsFalse()
    {
        var exists = await _repository.ExistsAsync(Guid.NewGuid());

        exists.Should().BeFalse();
    }
}
