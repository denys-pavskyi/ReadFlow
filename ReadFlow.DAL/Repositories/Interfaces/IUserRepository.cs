using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);

    Task<List<BookRating>> GetUserRatingsAsync(Guid userId);
    Task<decimal> GetPlatformAverageRatingAsync();
    Task<List<(string GenreName, int Rating)>> GetUserGenreRatingsAsync(Guid userId);
}
