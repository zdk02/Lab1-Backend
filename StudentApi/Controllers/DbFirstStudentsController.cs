using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.DbFirst; 
namespace StudentApi.Controllers
{
    [Route("api/dbfirst/[controller]")]
    [ApiController]
    public class DbFirstStudentsController : ControllerBase
    {
        private readonly StudentDbFirstContext _context;

        public DbFirstStudentsController(StudentDbFirstContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }
    }
}