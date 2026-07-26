using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Analytics;
using ReadFlow.BLL.Helpers;
using ReadFlow.BLL.Services;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Queries.Analytics;

public class GetUserReadingStatsQueryHandler
    : IRequestHandler<GetUserReadingStatsQuery, Result<UserReadingStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AnalyticsService _analyticsService;

    public GetUserReadingStatsQueryHandler(
        IUnitOfWork unitOfWork,
        AnalyticsService analyticsService)
    {
        _unitOfWork = unitOfWork;
        _analyticsService = analyticsService;
    }

    public async Task<Result<UserReadingStatsDto>> Handle(
        GetUserReadingStatsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Verify user exists
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            return Result<UserReadingStatsDto>.Failure(
                Error.NotFound("User.NotFound", $"User with ID {request.UserId} was not found")
            );
        }

        // 2. Get reading pace analytics
        var readingPace = await _analyticsService.GetReadingPaceAsync(request.UserId);

        // 3. Get rating pattern analytics
        var ratingPattern = await _analyticsService.GetRatingPatternAsync(request.UserId);

        // 4. Get genre breakdown
        var genreBreakdown = await _analyticsService.GetGenreBreakdownAsync(request.UserId);

        // 5. Get reading goals
        var readingGoals = await ReadingGoalsCalculator.CalculateGoalsAsync(
            _unitOfWork,
            request.UserId
        );

        var stats = new UserReadingStatsDto
        {
            UserId = user.Id,
            Username = user.Username,
            ReadingPace = readingPace,
            RatingPattern = ratingPattern,
            GenreBreakdown = genreBreakdown,
            ReadingGoals = readingGoals
        };

        return Result<UserReadingStatsDto>.Success(stats);
    }
}
