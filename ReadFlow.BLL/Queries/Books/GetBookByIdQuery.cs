using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;

namespace ReadFlow.BLL.Queries.Books;

public record GetBookByIdQuery(Guid Id) : IRequest<Result<BookDetailDto>>;
