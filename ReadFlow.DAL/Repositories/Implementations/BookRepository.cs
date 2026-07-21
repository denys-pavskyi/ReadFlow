using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Book?> GetBookWithGenresAsync(Guid bookId)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.BookGenres)
            .ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }
}
