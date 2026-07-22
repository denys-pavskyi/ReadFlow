namespace ReadFlow.BLL.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }
    public List<Error> Errors { get; }

    protected Result(bool isSuccess, Error? error = null, List<Error>? errors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors ?? new List<Error>();

        if (error != null && !Errors.Contains(error))
        {
            Errors.Add(error);
        }
    }

    public static Result Success() => new(true);

    public static Result Failure(Error error) => new(false, error);

    public static Result Failure(params Error[] errors) => new(false, errors.FirstOrDefault(), errors.ToList());

    public static Result Failure(List<Error> errors) => new(false, errors.FirstOrDefault(), errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value = default, Error? error = null, List<Error>? errors = null)
        : base(isSuccess, error, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value);

    public static new Result<T> Failure(Error error) => new(false, default, error);

    public static new Result<T> Failure(params Error[] errors) => new(false, default, errors.FirstOrDefault(), errors.ToList());

    public static new Result<T> Failure(List<Error> errors) => new(false, default, errors.FirstOrDefault(), errors);
}
