using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ReadFlow.API.Models;

namespace ReadFlow.API.Filters;

public class DatabaseExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DatabaseExceptionFilter> _logger;

    public DatabaseExceptionFilter(ILogger<DatabaseExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update exception occurred");

            if (dbEx.InnerException?.Message.Contains("duplicate key") == true ||
                dbEx.InnerException?.Message.Contains("unique constraint") == true)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "A record with this information already exists",
                    Code = "Database.DuplicateKey",
                    TraceId = context.HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                };

                context.Result = new ConflictObjectResult(errorResponse);
                context.ExceptionHandled = true;
            }
        }
        else if (context.Exception is DbUpdateConcurrencyException concurrencyEx)
        {
            _logger.LogWarning(concurrencyEx, "Concurrency conflict occurred");

            var errorResponse = new ErrorResponse
            {
                Message = "The record was modified by another user. Please refresh and try again.",
                Code = "Database.ConcurrencyConflict",
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            context.Result = new ConflictObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
