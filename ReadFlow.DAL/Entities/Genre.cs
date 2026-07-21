using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Genre : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
