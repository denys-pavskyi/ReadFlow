using Microsoft.EntityFrameworkCore.Storage;
using ReadFlow.DAL.Data;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.DAL.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IBookRepository? _books;
    private IUserRepository? _users;
    private IGenreRepository? _genres;
    private IReviewRepository? _reviews;
    private ICommentRepository? _comments;
    private ICollectionRepository? _collections;
    private IPostRepository? _posts;
    private IReadingGoalRepository? _readingGoals;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IBookRepository Books => _books ??= new BookRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IGenreRepository Genres => _genres ??= new GenreRepository(_context);
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
    public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
    public ICollectionRepository Collections => _collections ??= new CollectionRepository(_context);
    public IPostRepository Posts => _posts ??= new PostRepository(_context);
    public IReadingGoalRepository ReadingGoals => _readingGoals ??= new ReadingGoalRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
