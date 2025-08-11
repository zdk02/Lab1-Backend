using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentApi.Features.Students.Commands
{
    public class UploadImageCommandHandler 
        : IRequestHandler<UploadImageCommand, UploadImageResult>
    {
        public async Task<UploadImageResult> Handle(
            UploadImageCommand request,
            CancellationToken cancellationToken)
        {
            var imageFile = request.ImageFile;
            var env = request.Env;

            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No file was uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException(
                    $"Invalid file type. Allowed types are: {string.Join(", ", allowedExtensions)}"
                );

            var uploadsFolder = Path.Combine(
                env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                "uploads"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, cancellationToken);
            }

            return new UploadImageResult
            {
                Message = "File uploaded successfully.",
                FileName = uniqueFileName,
                Url = $"/uploads/{uniqueFileName}"
            };
        }
    }
}