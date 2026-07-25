using AutoMapper;
using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.BLL.DTOs.Books;
using ReadFlow.DAL.Entities;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Commands.Books;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<BookDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id);
        if (book == null)
        {
            return Result<BookDto>.Failure(
                Error.NotFound("Book.NotFound", $"Book with ID {request.Id} was not found")
            );
        }

        if (!string.IsNullOrEmpty(request.BookDto.Title))
            book.Title = request.BookDto.Title;

        if (!string.IsNullOrEmpty(request.BookDto.Author))
            book.Author = request.BookDto.Author;

        if (request.BookDto.ISBN != null)
            book.ISBN = request.BookDto.ISBN;

        if (request.BookDto.Description != null)
            book.Description = request.BookDto.Description;

        if (request.BookDto.PublishedDate != null)
            book.PublishedDate = request.BookDto.PublishedDate;

        if (request.BookDto.PageCount != null)
            book.PageCount = request.BookDto.PageCount;

        if (request.BookDto.CoverImageUrl != null)
            book.CoverImageUrl = request.BookDto.CoverImageUrl;

        await _unitOfWork.SaveChangesAsync();

        var bookDto = _mapper.Map<BookDto>(book);

        return Result<BookDto>.Success(bookDto);
    }
}
