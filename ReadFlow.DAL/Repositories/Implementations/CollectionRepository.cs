using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
{
    public CollectionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Collection>> GetCollectionsByUserAsync(Guid userId)
    {
        return await _context.Collections
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
