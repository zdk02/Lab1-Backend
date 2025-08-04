using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using System.Globalization;
using StudentApi.Services;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _studentService.GetAll();

                if (students == null || !students.Any())
                    return NotFound(new { message = "No students found." });

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while retrieving students.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "ID must be a positive number." });

            try
            {
                var student = _studentService.GetById(id);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {id} not found." });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Name query parameter is required." });

            try
            {
                var matchedStudents = _studentService.Search(name);
                return Ok(matchedStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public IActionResult UpdateStudent([FromBody] UpdateStudentDto updatedStudent)
        {
            if (updatedStudent == null)
                return BadRequest(new { message = "Update data is required." });

            if (updatedStudent.Id <= 0 ||
                string.IsNullOrWhiteSpace(updatedStudent.Name) ||
                string.IsNullOrWhiteSpace(updatedStudent.Email))
                return BadRequest(new { message = "Invalid input data." });

            try
            {
                var student = _studentService.Update(updatedStudent);
                if (student == null)
                    return NotFound(new { message = $"Student with ID {updatedStudent.Id} not found." });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "ID must be a positive number." });

            try
            {
                var result = _studentService.Delete(id);
                if (!result)
                    return NotFound(new { message = $"Student with ID {id} not found." });

                return Ok(new { message = $"Student with ID {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Deletion failed.", error = ex.Message });
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile,
            [FromServices] IWebHostEnvironment env)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file was uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    message = $"Invalid file type. Allowed types are: {string.Join(", ", allowedExtensions)}"
                });
            }

            var uploadsFolder = Path.Combine(
                env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                "uploads"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return Ok(new
                {
                    message = "File uploaded successfully.",
                    fileName = uniqueFileName,
                    url = $"/uploads/{uniqueFileName}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while saving the file.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("date")]
        public IActionResult GetFormattedDate()
        {
            string culture = "en-US";
            string acceptLanguage = Request.Headers["Accept-Language"].ToString();

            try
            {
                if (!string.IsNullOrWhiteSpace(acceptLanguage))
                {
                    var selectedCulture = acceptLanguage.Split(',')[0].Trim();
                    var validCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                        .Select(c => c.Name)
                        .ToList();

                    if (!validCultures.Contains(selectedCulture))
                        throw new CultureNotFoundException($"Culture '{selectedCulture}' is not supported.");

                    culture = selectedCulture;
                }

                var formattedDate = DateTime.Now.ToString(new CultureInfo(culture));
                return Ok(new { date = formattedDate, culture });
            }
            catch (CultureNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}