namespace ERP.API.Middleware;

using ERP.Domain.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = "Ocorreu um erro ao processar a requisição. Tente novamente.",
            StatusCode = StatusCodes.Status500InternalServerError,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case BusinessException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = ex.Message;
                response.ErrorCode = ex.ErrorCode;
                break;

            case NotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = ex.Message;
                break;

            case ValidationException ex:
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                response.Message = "Validação falhou";
                response.Errors = ex.Errors;
                break;

            case AccessDeniedException ex:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.Message = ex.Message;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public IDictionary<string, string[]>? Errors { get; set; }
}
