using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
namespace StudentApi.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine("Hello incoming requesting");
        await _next(context);
    }
}