using AutoMapper;
using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;
using ReadFlow.BLL.Helpers;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Queries.Books;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookDetailDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetBookWithGenresAsync(request.Id);
        if (book == null)
        {
            return Result<BookDetailDto>.Failure(
                Error.NotFound("Book.NotFound", $"Book with ID {request.Id} was not found")
            );
        }

        var bookDto = _mapper.Map<BookDetailDto>(book);

        var ratings = await _unitOfWork.Books.FindAsync(b => b.Id == request.Id);
        var bookRatings = ratings.SelectMany(b => b.Ratings);

        if (bookRatings.Any())
        {
            bookDto.AverageRating = RatingCalculator.CalculateAverageRating(bookRatings);
            bookDto.RatingCount = bookRatings.Count();
        }

        return Result<BookDetailDto>.Success(bookDto);
    }
}
