using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class PostLike : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}
