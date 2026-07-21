using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class BookGenre
{
    [Required]
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
}
