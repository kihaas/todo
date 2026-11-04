using System.Text.Json;
using todo.Exceptions;

namespace todo.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            var (status, message) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                ConflictException => (StatusCodes.Status409Conflict, ex.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, ex.Message),
                ForbiddenException => (StatusCodes.Status403Forbidden, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
            };

            if (status == StatusCodes.Status500InternalServerError)
                _logger.LogError(ex, "Необработанная ошибка");

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json; charset=utf-8";

            var body = JsonSerializer.Serialize(new { status, error = message });
            await context.Response.WriteAsync(body);
        }
    }
}