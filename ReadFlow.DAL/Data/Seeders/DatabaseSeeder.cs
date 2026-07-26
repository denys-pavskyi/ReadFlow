using Microsoft.Extensions.Logging;

namespace ReadFlow.DAL.Data.Seeders;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly List<IDataSeeder> _seeders;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
        _seeders = new List<IDataSeeder>
        {
            new GenreSeeder(),
            new UserSeeder(),
            new BookSeeder(),
            new BookRatingSeeder(),
            new ReviewSeeder(),
            new CommentSeeder(),
            new ReadingGoalSeeder()
        };
    }

    public async Task SeedAllAsync()
    {
        foreach (var seeder in _seeders)
        {
            var seederName = seeder.GetType().Name;
            Console.WriteLine($"Seeding {seederName}...");
            await seeder.SeedAsync(_context);
            Console.WriteLine($"Completed {seederName}");
        }
    }
}
