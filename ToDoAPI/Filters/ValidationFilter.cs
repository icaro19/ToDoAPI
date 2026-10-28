using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Models;

namespace WebAPI.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var errors =
                context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

            context.Result =
                new BadRequestObjectResult(
                    ApiResponse<IEnumerable<string>>
                        .Fail(
                            string.Join("; ", errors)));
        }

        public void OnActionExecuted(
            ActionExecutedContext context)
        {
        }
    }
}
