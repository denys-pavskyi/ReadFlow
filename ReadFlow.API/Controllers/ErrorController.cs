using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReadFlow.API.Models;

namespace ReadFlow.API.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    public IActionResult HandleError()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        _logger.LogError(exception, "Unhandled exception occurred");

        var errorResponse = new ErrorResponse
        {
            Message = "An error occurred processing your request",
            Code = "Internal.Error",
            TraceId = HttpContext.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        return StatusCode(500, errorResponse);
    }

    [Route("/error-dev")]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        return Problem(
            detail: exception?.StackTrace,
            title: exception?.Message,
            statusCode: 500
        );
    }
}
