using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;

namespace ReadFlow.BLL.Commands.Books;

public record CreateBookCommand(CreateBookDto BookDto) : IRequest<Result<BookDto>>;
