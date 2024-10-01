using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILog _logger;

    public CustomExceptionFilter()
    {
        _logger = LogManager.GetLogger(typeof(CustomExceptionFilter));
    }

    public void OnException(ExceptionContext context)
    {
        // Log the exception details
        _logger.Error("An unhandled exception occurred.", context.Exception);

        // Set the result as a JSON response with a generic error message
        context.Result = new JsonResult(new { message = "Oops, something went wrong." })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
