namespace ReadFlow.BLL.DTOs.Comments;

public class CreateCommentDto
{
    public Guid BookId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public string Content { get; set; } = string.Empty;
}
