using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;

namespace ReadFlow.BLL.Queries.Books;

public record SearchBooksQuery(string SearchTerm, int Page = 1, int PageSize = 20) : IRequest<Result<IEnumerable<BookDto>>>;
