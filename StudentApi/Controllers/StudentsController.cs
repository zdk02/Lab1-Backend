using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using MediatR;
using StudentApi.Models;
using StudentApi.Features.Students.Queries;
using StudentApi.Features.Students.Commands;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _mediator.Send(new GetAllStudentsQuery());

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

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetStudentById(long id)
        {
            if (id <= 0)
                return BadRequest(new { message = "ID must be a positive number." });

            try
            {
                var student = await _mediator.Send(new GetStudentByIdQuery(id));
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
        public async Task<IActionResult> SearchStudents([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Name query parameter is required." });

            try
            {
                var matchedStudents = await _mediator.Send(new SearchStudentsQuery(name.Trim()));
                return Ok(matchedStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDto updatedStudent)
        {
            if (updatedStudent == null)
                return BadRequest(new { message = "Update data is required." });

            if (updatedStudent.Id <= 0 ||
                string.IsNullOrWhiteSpace(updatedStudent.Name) ||
                string.IsNullOrWhiteSpace(updatedStudent.Email))
                return BadRequest(new { message = "Invalid input data." });

            try
            {
                // optional trim
                updatedStudent = new UpdateStudentDto
                {
                    Id = updatedStudent.Id,
                    Name = updatedStudent.Name.Trim(),
                    Email = updatedStudent.Email.Trim()
                };

                var student = await _mediator.Send(new UpdateStudentCommand(updatedStudent));
                if (student == null)
                    return NotFound(new { message = $"Student with ID {updatedStudent.Id} not found." });

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            if (id <= 0)
                return BadRequest(new { message = "ID must be a positive number." });

            try
            {
                var result = await _mediator.Send(new DeleteStudentCommand(id));
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                var result = await _mediator.Send(new UploadImageCommand { ImageFile = imageFile, Env = env });
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetFormattedDate()
        {
            try
            {
                var result = await _mediator.Send(new GetFormattedDateQuery
                {
                    AcceptLanguage = Request.Headers["Accept-Language"].ToString()
                });

                return Ok(result);
            }
            catch (CultureNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}