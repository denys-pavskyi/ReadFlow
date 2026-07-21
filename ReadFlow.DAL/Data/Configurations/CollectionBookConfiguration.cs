using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadFlow.DAL.Entities;

namespace ReadFlow.DAL.Data.Configurations;

public class CollectionBookConfiguration : IEntityTypeConfiguration<CollectionBook>
{
    public void Configure(EntityTypeBuilder<CollectionBook> builder)
    {
        builder.HasKey(cb => new { cb.CollectionId, cb.BookId });

        builder.HasOne(cb => cb.Collection)
            .WithMany(c => c.CollectionBooks)
            .HasForeignKey(cb => cb.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Book)
            .WithMany(b => b.CollectionBooks)
            .HasForeignKey(cb => cb.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
