using Microsoft.EntityFrameworkCore;
using ReadFlow.BLL.DTOs.Analytics;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Services;

public class AnalyticsService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnalyticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReadingPaceDto> GetReadingPaceAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        // Get all user ratings from repository
        var userRatings = await _unitOfWork.Users.GetUserRatingsAsync(userId);

        // Total books read (distinct books)
        var totalBooks = userRatings
            .Select(r => r.BookId)
            .Distinct()
            .Count();

        if (totalBooks == 0)
        {
            return new ReadingPaceDto
            {
                TotalBooksRead = 0,
                BooksReadThisMonth = 0,
                BooksReadThisYear = 0,
                AverageBooksPerMonth = 0,
                AverageBooksPerYear = 0
            };
        }

        // Books read this month
        var booksThisMonth = userRatings
            .Where(r => r.CreatedAt >= startOfMonth)
            .Select(r => r.BookId)
            .Distinct()
            .Count();

        // Books read this year
        var booksThisYear = userRatings
            .Where(r => r.CreatedAt >= startOfYear)
            .Select(r => r.BookId)
            .Distinct()
            .Count();

        // First and most recent rating dates
        var firstRating = userRatings
            .OrderBy(r => r.CreatedAt)
            .Select(r => r.CreatedAt)
            .FirstOrDefault();

        var mostRecentRating = userRatings
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.CreatedAt)
            .FirstOrDefault();

        // Calculate duration and averages
        var durationDays = (int)(now - firstRating).TotalDays;
        var durationMonths = durationDays > 0 ? durationDays / 30.0m : 1;
        var durationYears = durationDays > 0 ? durationDays / 365.0m : 1;

        return new ReadingPaceDto
        {
            TotalBooksRead = totalBooks,
            BooksReadThisMonth = booksThisMonth,
            BooksReadThisYear = booksThisYear,
            AverageBooksPerMonth = Math.Round(totalBooks / durationMonths, 2),
            AverageBooksPerYear = Math.Round(totalBooks / durationYears, 2),
            FirstBookReadDate = firstRating,
            MostRecentBookReadDate = mostRecentRating,
            ReadingDurationDays = durationDays
        };
    }

    public async Task<RatingPatternDto> GetRatingPatternAsync(Guid userId)
    {
        // Get user ratings from repository
        var userRatings = await _unitOfWork.Users.GetUserRatingsAsync(userId);

        if (userRatings.Count == 0)
        {
            return new RatingPatternDto
            {
                UserAverageRating = 0,
                PlatformAverageRating = 0,
                RatingDifference = 0,
                RatingDistribution = new int[10, 2]
            };
        }

        // User average rating
        var userAvg = (decimal)userRatings.Average(r => r.Rating);

        // Platform average rating (all users)
        var platformAvg = await _unitOfWork.Users.GetPlatformAverageRatingAsync();

        // Rating distribution - MULTI-DIMENSIONAL ARRAY
        var ratingDistribution = new int[10, 2];
        for (int i = 0; i < 10; i++)
        {
            var rating = i + 1; // Ratings are 1-10
            var count = userRatings.Count(r => r.Rating == rating);
            ratingDistribution[i, 0] = rating;
            ratingDistribution[i, 1] = count;
        }

        // Genre rating averages - get from repository
        var genreRatingData = await _unitOfWork.Users.GetUserGenreRatingsAsync(userId);

        var genreRatingAverages = genreRatingData
            .GroupBy(x => x.GenreName)
            .ToDictionary(
                g => g.Key,
                g => Math.Round((decimal)g.Average(x => x.Rating), 2)
            );

        // Top rated genres
        var topRatedGenres = genreRatingAverages
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .Select(kvp => new GenreRatingDto
            {
                GenreName = kvp.Key,
                AverageRating = kvp.Value,
                BookCount = userRatings
                    .Count(r => r.Book.BookGenres.Any(bg => bg.Genre.Name == kvp.Key))
            })
            .ToList();

        return new RatingPatternDto
        {
            UserAverageRating = Math.Round(userAvg, 2),
            PlatformAverageRating = Math.Round(platformAvg, 2),
            RatingDifference = Math.Round(userAvg - platformAvg, 2),
            RatingDistribution = ratingDistribution,
            GenreRatingAverages = genreRatingAverages,
            TopRatedGenres = topRatedGenres
        };
    }

    public async Task<GenreBreakdownDto> GetGenreBreakdownAsync(Guid userId)
    {
        // Get genre ratings from repository
        var genreRatingData = await _unitOfWork.Users.GetUserGenreRatingsAsync(userId);

        var genreStats = genreRatingData
            .GroupBy(x => x.GenreName)
            .Select(g => new
            {
                GenreName = g.Key,
                Count = g.Count(),
                AverageRating = (decimal)g.Average(x => x.Rating)
            })
            .ToList();

        var totalGenreAssignments = genreStats.Sum(g => g.Count);

        var genreStatsDto = genreStats
            .Select(g => new GenreStatsDto
            {
                GenreName = g.GenreName,
                BookCount = g.Count,
                Percentage = totalGenreAssignments > 0
                    ? Math.Round((decimal)g.Count / totalGenreAssignments * 100, 2)
                    : 0,
                AverageRating = Math.Round(g.AverageRating, 2)
            })
            .OrderByDescending(g => g.BookCount)
            .ToList();

        // Favorite genres: highest rated + most read
        var favoriteGenres = genreStatsDto
            .OrderByDescending(g => g.AverageRating)
            .ThenByDescending(g => g.BookCount)
            .Take(3)
            .Select(g => g.GenreName)
            .ToList();

        return new GenreBreakdownDto
        {
            GenreStats = genreStatsDto,
            FavoriteGenres = favoriteGenres
        };
    }
}
