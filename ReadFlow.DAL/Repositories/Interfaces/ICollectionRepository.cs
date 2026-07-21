using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Repositories.Interfaces;

public interface ICollectionRepository : IRepository<Collection>
{
    Task<IEnumerable<Collection>> GetCollectionsByUserAsync(Guid userId);
}
