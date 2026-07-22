namespace ReadFlow.BLL.DTOs.Ratings;

public class CreateRatingDto
{
    public Guid BookId { get; set; }
    public int Rating { get; set; }
}
