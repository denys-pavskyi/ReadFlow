using MediatR;
using ReadFlow.BLL.Common;
using ReadFlow.DAL.Repositories.Interfaces;

namespace ReadFlow.BLL.Commands.Books;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id);
        if (book == null)
        {
            return Result.Failure(
                Error.NotFound("Book.NotFound", $"Book with ID {request.Id} was not found")
            );
        }

        await _unitOfWork.Books.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
