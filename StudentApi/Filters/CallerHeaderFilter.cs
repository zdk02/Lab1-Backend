using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentApi.Filters
{
    public class CallerHeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var hasCaller = context.HttpContext.Request.Headers.TryGetValue("Caller", out var callerHeader);
            if (!hasCaller || string.IsNullOrWhiteSpace(callerHeader))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "Missing or empty Caller header."
                });
                return;
            }

            if (callerHeader == "Unknown")
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = "Invalid caller header value."
                });
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}