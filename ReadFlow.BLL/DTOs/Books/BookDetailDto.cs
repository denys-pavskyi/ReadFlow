using ReadFlow.BLL.DTOs.Genres;

namespace ReadFlow.BLL.DTOs.Books;

public class BookDetailDto : BookDto
{
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int? PageCount { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
}
