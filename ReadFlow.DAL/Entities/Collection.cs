using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Collection : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsPublic { get; set; } = false;

    // Navigation properties
    public ICollection<CollectionBook> CollectionBooks { get; set; } = new List<CollectionBook>();
}
