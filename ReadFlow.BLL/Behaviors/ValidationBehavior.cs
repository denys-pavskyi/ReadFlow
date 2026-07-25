using FluentValidation;
using MediatR;
using ReadFlow.BLL.Common;

namespace ReadFlow.BLL.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var errors = failures.Select(f =>
                Error.Validation($"Validation.{f.PropertyName}", f.ErrorMessage)
            ).ToList();

            var resultType = typeof(TResponse);
            if (resultType.IsGenericType)
            {
                var valueType = resultType.GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(valueType)
                    .GetMethod(nameof(Result<object>.Failure), new[] { typeof(List<Error>) });

                return (TResponse)failureMethod!.Invoke(null, new object[] { errors })!;
            }
            else
            {
                var failureMethod = typeof(Result).GetMethod(nameof(Result.Failure), new[] { typeof(List<Error>) });
                return (TResponse)failureMethod!.Invoke(null, new object[] { errors })!;
            }
        }

        return await next();
    }
}
