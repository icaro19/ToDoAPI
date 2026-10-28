using WebAPI.Middlewares;

namespace WebAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UseApiPipeline(
            this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<
                ExceptionHandlingMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
