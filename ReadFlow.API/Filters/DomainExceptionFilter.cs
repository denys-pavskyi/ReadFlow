using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReadFlow.API.Models;
using ReadFlow.BLL.Exceptions;

namespace ReadFlow.API.Filters;

public class DomainExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException notFoundEx)
        {
            var errorResponse = new ErrorResponse
            {
                Message = notFoundEx.Message,
                Code = notFoundEx.Code,
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            context.Result = new NotFoundObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
        else if (context.Exception is ConflictException conflictEx)
        {
            var errorResponse = new ErrorResponse
            {
                Message = conflictEx.Message,
                Code = conflictEx.Code,
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            context.Result = new ConflictObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
