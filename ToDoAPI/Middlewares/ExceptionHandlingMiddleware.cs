using Domain.Validation;
using WebAPI.Models;

namespace WebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainExceptionValidation ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (ApplicationException ex)
            {
                await HandleException(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message);
            }
            catch (Exception)
            {
                await HandleException(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor.");
            }
        }

        private static async Task HandleException(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.StatusCode =
                statusCode;

            context.Response.ContentType =
                "application/json";

            await context.Response.WriteAsJsonAsync(
                new ErrorResponse
                {
                    Message = message
                });
        }
    }
}
