using Microsoft.AspNetCore.Mvc;
using ReadFlow.API.Models;
using ReadFlow.BLL.Common;

namespace ReadFlow.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return ToErrorActionResult(result);
    }

    protected IActionResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return ToErrorActionResult(result);
    }

    private IActionResult ToErrorActionResult(Result result)
    {
        var error = result.Errors.FirstOrDefault() ?? result.Error;

        if (error == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Message = "An unexpected error occurred",
                Code = "Internal.Unknown",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var errorResponse = new ErrorResponse
        {
            Message = error.Message,
            Code = error.Code,
            Errors = result.Errors.Select(e => new ErrorDetail
            {
                Code = e.Code,
                Message = e.Message,
                Field = ExtractFieldName(e.Code)
            }).ToList(),
            TraceId = HttpContext.TraceIdentifier
        };

        return error.Type switch
        {
            ErrorType.NotFound => NotFound(errorResponse),
            ErrorType.Validation => BadRequest(errorResponse),
            ErrorType.Conflict => Conflict(errorResponse),
            ErrorType.Unauthorized => Unauthorized(errorResponse),
            ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, errorResponse),
            ErrorType.BadRequest => BadRequest(errorResponse),
            _ => StatusCode(StatusCodes.Status500InternalServerError, errorResponse)
        };
    }

    private string? ExtractFieldName(string errorCode)
    {
        var parts = errorCode.Split('.');
        return parts.Length > 1 ? parts[1] : null;
    }
}
