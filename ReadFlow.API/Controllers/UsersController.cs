using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReadFlow.API.Models;
using ReadFlow.BLL.DTOs.Analytics;
using ReadFlow.BLL.Queries.Analytics;

namespace ReadFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get comprehensive reading analytics for a user
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>Reading statistics including pace, ratings, and genre breakdown</returns>
    [HttpGet("{userId}/analytics/reading-stats")]
    [ProducesResponseType(typeof(UserReadingStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReadingStats(Guid userId)
    {
        var query = new GetUserReadingStatsQuery(userId);
        var result = await _mediator.Send(query);
        return ToActionResult(result);
    }
}
