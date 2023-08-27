using System.Net;
using System.Text.Json;
using WebApi.Models;

namespace WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AccessViolationException avEx)
            {
                _logger.LogError($"A new violation exception has been thrown: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            var errorResponse = new ErrorResponse
            {
                Success = false
            };
            switch (exception)
            {
                case ApplicationException ex:
                    {
                        if (ex.Message.Contains("Invalid token"))
                        {
                            response.StatusCode = (int)HttpStatusCode.Forbidden;
                            errorResponse.Message = ex.Message;
                            break;
                        }
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                case KeyNotFoundException ex:
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                case InvalidCastException ex:
                    {
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = string.Join(string.Empty, "Cannot convert", ex.Message);
                        break;
                    }
                default:
                    {
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = string.Join(string.Empty, new string("Internal Server errors. Logs: "), exception.Message);
                        break;
                    }
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

    }

    public static class ExceptionHandleExtension
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }

}
