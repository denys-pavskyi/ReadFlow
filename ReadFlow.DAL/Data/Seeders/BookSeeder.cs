using Bogus;
using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Seeders;

public class BookSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (await context.Books.AnyAsync())
        {
            return;
        }
        var bookFaker = new Faker<Book>()
            .RuleFor(b => b.Title, f => $"{f.Commerce.ProductAdjective()} {f.Random.Word()} {f.Random.Word()}")
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
            .RuleFor(b => b.Description, f => f.Lorem.Paragraphs(3))
            .RuleFor(b => b.PublishedDate, f => f.Date.Past(50))
            .RuleFor(b => b.PageCount, f => f.Random.Int(100, 1000))
            .RuleFor(b => b.CoverImageUrl, f => f.Image.PicsumUrl());

        var books = bookFaker.Generate(300);

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();

        var allGenres = await context.Genres.ToListAsync();
        var bookGenres = new List<BookGenre>();

        foreach (var book in books)
        {
            var genreCount = new Random().Next(1, 6);
            var selectedGenres = allGenres.OrderBy(_ => Guid.NewGuid()).Take(genreCount);

            foreach (var genre in selectedGenres)
            {
                bookGenres.Add(new BookGenre
                {
                    BookId = book.Id,
                    GenreId = genre.Id
                });
            }
        }

        await context.BookGenres.AddRangeAsync(bookGenres);
        await context.SaveChangesAsync();
    }
}
