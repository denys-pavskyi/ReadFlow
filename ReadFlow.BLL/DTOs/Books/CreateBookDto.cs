namespace ReadFlow.BLL.DTOs.Books;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int? PageCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public List<Guid> GenreIds { get; set; } = new();
}
