using MediatR;

namespace StudentApi.Features.Students.Commands
{
    public class UploadImageCommand : IRequest<UploadImageResult>
    {
        public IFormFile ImageFile { get; set; }
        public IWebHostEnvironment Env { get; set; }
    }
    public class UploadImageResult
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}