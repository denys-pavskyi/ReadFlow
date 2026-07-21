using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class CollectionBook
{
    [Required]
    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; } = null!;

    [Required]
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public int? Order { get; set; }
}
