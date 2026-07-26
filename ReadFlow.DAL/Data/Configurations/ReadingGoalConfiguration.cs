using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Configurations;

public class ReadingGoalConfiguration : IEntityTypeConfiguration<ReadingGoal>
{
    public void Configure(EntityTypeBuilder<ReadingGoal> builder)
    {
        builder.ToTable("ReadingGoals");

        builder.HasKey(rg => rg.Id);

        builder.Property(rg => rg.UserId)
            .IsRequired();

        builder.Property(rg => rg.Year)
            .IsRequired();

        builder.Property(rg => rg.TargetBookCount)
            .IsRequired();

        builder.Property(rg => rg.Description)
            .HasMaxLength(500);

        // Unique constraint: one goal per user per year
        builder.HasIndex(rg => new { rg.UserId, rg.Year })
            .IsUnique()
            .HasDatabaseName("IX_ReadingGoals_UserId_Year");

        // Relationship with User
        builder.HasOne(rg => rg.User)
            .WithMany()
            .HasForeignKey(rg => rg.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
