using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class GenreSeeder : IDataSeeder
{
    private static readonly List<string> GenreNames = new()
    {
        "Fiction",
        "Non-Fiction",
        "Fantasy",
        "Science Fiction",
        "Mystery",
        "Thriller",
        "Romance",
        "Horror",
        "Historical Fiction",
        "Biography",
        "Autobiography",
        "Self-Help",
        "Business",
        "Philosophy",
        "Psychology",
        "Science",
        "History",
        "True Crime",
        "Poetry",
        "Drama",
        "Young Adult",
        "Children's Literature",
        "Graphic Novel",
        "Memoir",
        "Adventure",
        "Crime",
        "Dystopian",
        "Paranormal",
        "Contemporary",
        "Classic"
    };

    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.Genres.AnyAsync())
        {
            return;
        }

        var genres = GenreNames.Select(name => new Genre
        {
            Name = name,
            Description = $"{name} books and related content"
        }).ToList();

        await context.Genres.AddRangeAsync(genres);
        await context.SaveChangesAsync();
    }
}
