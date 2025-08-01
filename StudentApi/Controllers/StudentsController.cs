using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;
using System.Globalization;


namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(StudentRepository.Students);
        }
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = StudentRepository.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            return Ok(student); 
        }

        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name query parameter is required.");
            var matchedStudents = StudentRepository.Students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(matchedStudents);
        }

        [HttpGet("date")]
        public IActionResult GetFormattedDate()
        {
            var acceptLanguage = Request.Headers["Accept-Language"].ToString();
            var culture = "en-US";
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var languages = acceptLanguage.Split(',');
                if (languages.Length > 0)
                {
                    var selectedCulture = languages[0].Trim();

                    if (selectedCulture == "en-US" || selectedCulture == "es-ES" || selectedCulture == "fr-FR")
                    {
                        culture = selectedCulture;
                    }
                }
            }
            var formattedDate = DateTime.Now.ToString(new CultureInfo(culture));
            return Ok(new { date = formattedDate, culture });
        }

        [HttpPost("update")]
        public IActionResult UpdateStudent([FromBody] UpdateStudentDto updatedStudent)
        {
            var student = StudentRepository.Students.FirstOrDefault(s => s.Id == updatedStudent.Id);

            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {updatedStudent.Id} not found." });
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;

            return Ok(student);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile,
            [FromServices] IWebHostEnvironment env)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }
            var uploadsFolder = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
    
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
    
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/{uniqueFileName}";

            return Ok(new { message = "Image uploaded successfully!", path = relativePath });
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var result = StudentRepository.DeleteStudent(id);
            if (!result)
                return NotFound(new { message = $"Student with ID {id} not found." });

            return Ok(new { message = $"Student with ID {id} deleted successfully." });
        }
    }
}