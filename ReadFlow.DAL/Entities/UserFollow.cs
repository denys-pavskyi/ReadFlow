using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class UserFollow : BaseEntity
{
    [Required]
    public Guid FollowerId { get; set; }
    public User Follower { get; set; } = null!;

    [Required]
    public Guid FollowingId { get; set; }
    public User Following { get; set; } = null!;
}
