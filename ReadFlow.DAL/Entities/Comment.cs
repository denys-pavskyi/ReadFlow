using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class Comment : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public ICollection<CommentVote> Votes { get; set; } = new List<CommentVote>();
}
