namespace ReadFlow.BLL.DTOs.Books;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public decimal? AverageRating { get; set; }
    public int RatingCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
