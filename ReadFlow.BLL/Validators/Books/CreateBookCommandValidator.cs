using FluentValidation;
using ReadFlow.BLL.Commands.Books;

namespace ReadFlow.BLL.Validators.Books;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.BookDto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255).WithMessage("Title cannot exceed 255 characters");

        RuleFor(x => x.BookDto.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(255).WithMessage("Author cannot exceed 255 characters");

        RuleFor(x => x.BookDto.ISBN)
            .Matches(@"^(?:\d{10}|\d{13})$").WithMessage("ISBN must be 10 or 13 digits")
            .When(x => !string.IsNullOrEmpty(x.BookDto.ISBN));

        RuleFor(x => x.BookDto.PublishedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Published date cannot be in the future")
            .When(x => x.BookDto.PublishedDate.HasValue);

        RuleFor(x => x.BookDto.PageCount)
            .GreaterThan(0).WithMessage("Page count must be greater than 0")
            .When(x => x.BookDto.PageCount.HasValue);
    }
}
