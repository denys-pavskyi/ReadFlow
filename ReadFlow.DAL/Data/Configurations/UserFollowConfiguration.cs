using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Configurations;

public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
{
    public void Configure(EntityTypeBuilder<UserFollow> builder)
    {
        builder.HasKey(uf => uf.Id);

        builder.HasIndex(uf => new { uf.FollowerId, uf.FollowingId }).IsUnique();

        builder.HasOne(uf => uf.Follower)
            .WithMany()
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uf => uf.Following)
            .WithMany()
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Add check constraint to prevent self-follows (will be created in migration)
        builder.ToTable(t => t.HasCheckConstraint("CK_UserFollow_NoSelfFollow", "\"FollowerId\" <> \"FollowingId\""));
    }
}
