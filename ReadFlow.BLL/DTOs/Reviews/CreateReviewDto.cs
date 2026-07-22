namespace ReadFlow.BLL.DTOs.Reviews;

public class CreateReviewDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
