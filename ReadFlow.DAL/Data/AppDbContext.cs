using Microsoft.EntityFrameworkCore;
using ReadFlow.DAL.Data.Configurations;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookRating> BookRatings => Set<BookRating>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentVote> CommentVotes => Set<CommentVote>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionBook> CollectionBooks => Set<CollectionBook>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostLike> PostLikes => Set<PostLike>();
    public DbSet<UserFollow> UserFollows => Set<UserFollow>();
    public DbSet<ReadingGoal> ReadingGoals => Set<ReadingGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
        modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
        modelBuilder.ApplyConfiguration(new BookRatingConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new CommentVoteConfiguration());
        modelBuilder.ApplyConfiguration(new CollectionConfiguration());
        modelBuilder.ApplyConfiguration(new CollectionBookConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
        modelBuilder.ApplyConfiguration(new UserFollowConfiguration());
        modelBuilder.ApplyConfiguration(new ReadingGoalConfiguration());

        // Global query filter for soft deletes
        modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
