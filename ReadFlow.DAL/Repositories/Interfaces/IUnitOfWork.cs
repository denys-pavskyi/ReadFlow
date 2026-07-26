namespace ReadFlow.DAL.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    IUserRepository Users { get; }
    IGenreRepository Genres { get; }
    IReviewRepository Reviews { get; }
    ICommentRepository Comments { get; }
    ICollectionRepository Collections { get; }
    IPostRepository Posts { get; }
    IReadingGoalRepository ReadingGoals { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
