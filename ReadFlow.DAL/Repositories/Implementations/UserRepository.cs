using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<List<BookRating>> GetUserRatingsAsync(Guid userId)
    {
        return await _context.BookRatings
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r => r.Book)
                .ThenInclude(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .ToListAsync();
    }

    public async Task<decimal> GetPlatformAverageRatingAsync()
    {
        return await _context.BookRatings
            .AsNoTracking()
            .AverageAsync(r => (decimal)r.Rating);
    }

    public async Task<List<(string GenreName, int Rating)>> GetUserGenreRatingsAsync(Guid userId)
    {
        return await _context.BookRatings
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r => r.Book)
                .ThenInclude(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .SelectMany(r => r.Book.BookGenres.Select(bg => new
            {
                GenreName = bg.Genre.Name,
                Rating = r.Rating
            }))
            .Select(x => ValueTuple.Create(x.GenreName, x.Rating))
            .ToListAsync();
    }
}
