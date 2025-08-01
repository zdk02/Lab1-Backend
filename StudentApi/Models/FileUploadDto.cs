using Microsoft.AspNetCore.Http;

namespace StudentApi.Models
{
    public class FileUploadDto
    {
        public IFormFile Image { get; set; }
    }
}