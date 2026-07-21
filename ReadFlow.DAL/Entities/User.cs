using System.ComponentModel.DataAnnotations;
using ReadFlow.DAL.Enums;

namespace ReadFlow.DAL.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? ProfilePictureUrl { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.User;

    // Navigation properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    public ICollection<BookRating> Ratings { get; set; } = new List<BookRating>();
    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    public ICollection<CommentVote> CommentVotes { get; set; } = new List<CommentVote>();
}
