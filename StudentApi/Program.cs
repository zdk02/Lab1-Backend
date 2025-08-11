using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Services;
using StudentApi.Filters;
using StudentApi.Middlewares;
using StudentApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CallerHeaderFilter>();
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
    db.Database.Migrate();
    SeedData.EnsureSeed(db);
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();

static class SeedData
{
    public static void EnsureSeed(StudentDbContext db)
    {
        if (!db.Students.Any())
        {
            db.Students.AddRange(
                new StudentApi.Models.Student { Id = 1, Name = "Alice", Email = "alice@test.com" },
                new StudentApi.Models.Student { Id = 2, Name = "Bob",   Email = "bob@test.com"   },
                new StudentApi.Models.Student { Id = 3, Name = "Carla", Email = "carla@test.com" }
            );
            db.SaveChanges();
        }
    }
}