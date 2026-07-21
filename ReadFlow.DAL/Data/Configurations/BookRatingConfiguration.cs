using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Configurations;

public class BookRatingConfiguration : IEntityTypeConfiguration<BookRating>
{
    public void Configure(EntityTypeBuilder<BookRating> builder)
    {
        builder.HasKey(br => br.Id);

        builder.HasIndex(br => new { br.UserId, br.BookId }).IsUnique();

        builder.HasOne(br => br.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(br => br.Book)
            .WithMany(b => b.Ratings)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
