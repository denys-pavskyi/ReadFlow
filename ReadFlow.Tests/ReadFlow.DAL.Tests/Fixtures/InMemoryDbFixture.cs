using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;

namespace ReadFlow.DAL.Tests.Fixtures;

public class InMemoryDbFixture : IDisposable
{
    private readonly string _databaseName;
    public DbContextOptions<AppDbContext> Options { get; }

    public InMemoryDbFixture()
    {
        _databaseName = $"TestDb_{Guid.NewGuid()}";

        Options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;
    }

    public AppDbContext CreateContext()
    {
        var context = new AppDbContext(Options);
        context.Database.EnsureCreated();
        return context;
    }

    public void Dispose()
    {
        using var context = new AppDbContext(Options);
        context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}
