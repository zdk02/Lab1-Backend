using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentApi.Services;
using StudentApi.Filters;
using StudentApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CallerHeaderFilter>();
});

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();