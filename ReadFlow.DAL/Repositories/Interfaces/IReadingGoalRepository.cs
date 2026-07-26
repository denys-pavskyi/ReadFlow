using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Repositories.Interfaces;

public interface IReadingGoalRepository : IRepository<ReadingGoal>
{
    Task<ReadingGoal?> GetGoalForYearAsync(Guid userId, int year);
}
