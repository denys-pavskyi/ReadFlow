using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class CommentVote : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public Guid CommentId { get; set; }
    public Comment Comment { get; set; } = null!;

    [Required]
    public bool IsUpvote { get; set; }
}
