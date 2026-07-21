using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class BookRating : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    [Range(1, 10)]
    public int Rating { get; set; }
}
