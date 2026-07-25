using MediatR;
using ReadFlow.BLL.Common;

namespace ReadFlow.BLL.Commands.Books;

public record DeleteBookCommand(Guid Id) : IRequest<Result>;
