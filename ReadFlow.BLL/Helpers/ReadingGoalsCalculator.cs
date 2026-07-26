using Microsoft.EntityFrameworkCore;
using ReadFlow.BLL.DTOs.Analytics;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Helpers;

public static class ReadingGoalsCalculator
{
    public static async Task<ReadingGoalsDto?> CalculateGoalsAsync(
        IUnitOfWork unitOfWork,
        Guid userId)
    {
        var currentYear = DateTime.UtcNow.Year;

        // Get user's goal for current year
        var goal = await unitOfWork.ReadingGoals.GetGoalForYearAsync(userId, currentYear);

        if (goal == null)
            return null;

        var now = DateTime.UtcNow;
        var startOfYear = new DateTime(now.Year, 1, 1);

        // Count books read this year
        var booksReadThisYear = await unitOfWork.Books
            .CountAsync(b => b.Ratings.Any(rating =>
                rating.UserId == userId &&
                rating.CreatedAt >= startOfYear));

        var daysInYear = DateTime.IsLeapYear(now.Year) ? 366 : 365;
        var daysElapsed = (now - startOfYear).Days + 1; // +1 to include today
        var daysRemaining = Math.Max(0, daysInYear - daysElapsed);

        var booksRemaining = Math.Max(0, goal.TargetBookCount - booksReadThisYear);
        var dailyPaceRequired = daysRemaining > 0
            ? (decimal)booksRemaining / daysRemaining
            : 0;

        var expectedByNow = (decimal)goal.TargetBookCount / daysInYear * daysElapsed;
        var onTrack = booksReadThisYear >= expectedByNow;

        var progressPercentage = goal.TargetBookCount > 0
            ? (decimal)booksReadThisYear / goal.TargetBookCount * 100
            : 0;

        return new ReadingGoalsDto
        {
            YearlyGoal = goal.TargetBookCount,
            BooksReadTowardGoal = booksReadThisYear,
            ProgressPercentage = Math.Round(progressPercentage, 2),
            BooksRemaining = booksRemaining,
            DailyPaceRequired = Math.Round(dailyPaceRequired, 3),
            OnTrack = onTrack
        };
    }
}
