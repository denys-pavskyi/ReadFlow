namespace ReadFlow.BLL.DTOs.Analytics;

public class UserReadingStatsDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;

    public ReadingPaceDto ReadingPace { get; set; } = new();
    public RatingPatternDto RatingPattern { get; set; } = new();
    public GenreBreakdownDto GenreBreakdown { get; set; } = new();
    public ReadingGoalsDto? ReadingGoals { get; set; }
}

public class ReadingPaceDto
{
    public int TotalBooksRead { get; set; }
    public int BooksReadThisMonth { get; set; }
    public int BooksReadThisYear { get; set; }
    public decimal AverageBooksPerMonth { get; set; }
    public decimal AverageBooksPerYear { get; set; }
    public DateTime? FirstBookReadDate { get; set; }
    public DateTime? MostRecentBookReadDate { get; set; }
    public int ReadingDurationDays { get; set; }
}

public class RatingPatternDto
{
    public decimal UserAverageRating { get; set; }
    public decimal PlatformAverageRating { get; set; }
    public decimal RatingDifference { get; set; }
    public int[,] RatingDistribution { get; set; } = new int[10, 2];

    public Dictionary<string, decimal> GenreRatingAverages { get; set; } = new();
    public List<GenreRatingDto> TopRatedGenres { get; set; } = new();
}

public class GenreRatingDto
{
    public string GenreName { get; set; } = string.Empty;
    public decimal AverageRating { get; set; }
    public int BookCount { get; set; }
}

public class GenreBreakdownDto
{
    public List<GenreStatsDto> GenreStats { get; set; } = new();
    public List<string> FavoriteGenres { get; set; } = new();
}

public class GenreStatsDto
{
    public string GenreName { get; set; } = string.Empty;
    public int BookCount { get; set; }
    public decimal Percentage { get; set; }
    public decimal AverageRating { get; set; }
}

public class ReadingGoalsDto
{
    public int YearlyGoal { get; set; }
    public int BooksReadTowardGoal { get; set; }
    public decimal ProgressPercentage { get; set; }
    public int BooksRemaining { get; set; }
    public decimal DailyPaceRequired { get; set; }
    public bool OnTrack { get; set; }
}
