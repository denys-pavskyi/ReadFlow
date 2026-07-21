using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Post : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
}
