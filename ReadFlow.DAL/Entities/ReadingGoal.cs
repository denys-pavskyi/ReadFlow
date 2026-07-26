using System.ComponentModel.DataAnnotations;

namespace ReadFlow.DAL.Entities;

public class ReadingGoal : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int Year { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Target book count must be at least 1")]
    public int TargetBookCount { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
