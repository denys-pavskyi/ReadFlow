using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Book : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ISBN { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime? PublishedDate { get; set; }

    public int? PageCount { get; set; }

    [MaxLength(500)]
    public string? CoverImageUrl { get; set; }

    // Navigation properties
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    public ICollection<BookRating> Ratings { get; set; } = new List<BookRating>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<CollectionBook> CollectionBooks { get; set; } = new List<CollectionBook>();
}
