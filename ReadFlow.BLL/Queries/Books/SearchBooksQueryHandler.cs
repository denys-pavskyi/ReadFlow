using AutoMapper;
using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;
using ReadFlow.BLL.Helpers;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Queries.Books;

public class SearchBooksQueryHandler : IRequestHandler<SearchBooksQuery, Result<IEnumerable<BookDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BookDto>>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm.ToLower();

        var books = await _unitOfWork.Books.FindAsync(b =>
            b.Title.ToLower().Contains(searchTerm) ||
            b.Author.ToLower().Contains(searchTerm) ||
            (b.ISBN != null && b.ISBN.Contains(searchTerm)));

        var pagedBooks = books
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var bookDtos = new List<BookDto>();

        foreach (var book in pagedBooks)
        {
            var bookDto = _mapper.Map<BookDto>(book);

            if (book.Ratings.Any())
            {
                bookDto.AverageRating = RatingCalculator.CalculateAverageRating(book.Ratings);
                bookDto.RatingCount = book.Ratings.Count;
            }

            bookDtos.Add(bookDto);
        }

        return Result<IEnumerable<BookDto>>.Success(bookDtos);
    }
}
