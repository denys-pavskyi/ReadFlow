using AutoMapper;
using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Commands.Books;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.BookDto.ISBN))
        {
            var existingBooks = await _unitOfWork.Books.FindAsync(b => b.ISBN == request.BookDto.ISBN);
            if (existingBooks.Any())
            {
                return Result<BookDto>.Failure(
                    Error.Conflict("Book.ISBNExists", $"Book with ISBN {request.BookDto.ISBN} already exists")
                );
            }
        }

        var book = _mapper.Map<Book>(request.BookDto);

        if (request.BookDto.GenreIds.Any())
        {
            foreach (var genreId in request.BookDto.GenreIds)
            {
                var genre = await _unitOfWork.Genres.GetByIdAsync(genreId);
                if (genre == null)
                {
                    return Result<BookDto>.Failure(
                        Error.NotFound("Genre.NotFound", $"Genre with ID {genreId} was not found")
                    );
                }

                book.BookGenres.Add(new BookGenre
                {
                    BookId = book.Id,
                    GenreId = genreId
                });
            }
        }

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        var bookDto = _mapper.Map<BookDto>(book);

        return Result<BookDto>.Success(bookDto);
    }
}
