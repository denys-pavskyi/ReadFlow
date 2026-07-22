namespace ReadFlow.BLL.DTOs.Books;

public class UpdateBookDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int? PageCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public List<Guid>? GenreIds { get; set; }
}
