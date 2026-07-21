using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetCommentsByBookAsync(Guid bookId)
    {
        return await _context.Comments
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.Votes)
            .Where(c => c.BookId == bookId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
