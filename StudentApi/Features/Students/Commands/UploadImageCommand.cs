using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace StudentApi.Features.Students.Commands
{
    public class UploadImageCommand : IRequest<UploadImageResult>
    {
        public IFormFile ImageFile { get; set; } = default!;
        public IWebHostEnvironment Env { get; set; } = default!;
    }

    public class UploadImageResult
    {
        public string Message { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}