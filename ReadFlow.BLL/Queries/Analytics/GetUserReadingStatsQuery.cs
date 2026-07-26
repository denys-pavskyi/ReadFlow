using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Analytics;

namespace ReadFlow.BLL.Queries.Analytics;

public record GetUserReadingStatsQuery(Guid UserId) : IRequest<Result<UserReadingStatsDto>>;
