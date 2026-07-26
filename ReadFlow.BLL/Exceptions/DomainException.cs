namespace ReadFlow.BLL.Exceptions;

public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    public DomainException(string code, string message, Exception inner)
        : base(message, inner)
    {
        Code = code;
    }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base("NotFound", $"{entityName} with id '{key}' was not found")
    {
    }
}

public class ConflictException : DomainException
{
    public ConflictException(string message)
        : base("Conflict", message)
    {
    }
}
