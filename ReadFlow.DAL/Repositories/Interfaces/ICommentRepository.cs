using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Repositories.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByBookAsync(Guid bookId);
}
