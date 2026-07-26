using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReadFlow.API.Models;

namespace ReadFlow.API.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationEx)
        {
            var errors = validationEx.Errors.Select(e => new ErrorDetail
            {
                Code = $"Validation.{e.PropertyName}",
                Message = e.ErrorMessage,
                Field = e.PropertyName
            }).ToList();

            var errorResponse = new ErrorResponse
            {
                Message = "Validation failed",
                Code = "Validation.Failed",
                Errors = errors,
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            context.Result = new BadRequestObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
