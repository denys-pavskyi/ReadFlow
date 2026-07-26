using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class ReadingGoalRepository : BaseRepository<ReadingGoal>, IReadingGoalRepository
{
    public ReadingGoalRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ReadingGoal?> GetGoalForYearAsync(Guid userId, int year)
    {
        return await _context.ReadingGoals
            .AsNoTracking()
            .FirstOrDefaultAsync(rg => rg.UserId == userId && rg.Year == year);
    }
}
