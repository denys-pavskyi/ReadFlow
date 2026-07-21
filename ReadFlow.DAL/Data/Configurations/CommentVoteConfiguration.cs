using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Configurations;

public class CommentVoteConfiguration : IEntityTypeConfiguration<CommentVote>
{
    public void Configure(EntityTypeBuilder<CommentVote> builder)
    {
        builder.HasKey(cv => cv.Id);

        builder.HasIndex(cv => new { cv.UserId, cv.CommentId }).IsUnique();

        builder.HasOne(cv => cv.User)
            .WithMany(u => u.CommentVotes)
            .HasForeignKey(cv => cv.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cv => cv.Comment)
            .WithMany(c => c.Votes)
            .HasForeignKey(cv => cv.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
