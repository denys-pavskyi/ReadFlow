namespace ReadFlow.API.Models;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<ErrorDetail> Errors { get; set; } = new();
    public string TraceId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ErrorDetail
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Field { get; set; }
}
