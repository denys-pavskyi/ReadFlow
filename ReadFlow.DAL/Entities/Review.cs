using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Review : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;
}
