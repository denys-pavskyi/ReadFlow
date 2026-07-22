namespace ReadFlow.BLL.DTOs.Collections;

public class CollectionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
