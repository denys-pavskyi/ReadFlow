using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Repositories.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetBookWithGenresAsync(Guid bookId);
}
