using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.DbFirst;      
using Student = StudentApi.DbFirst.Entities.Student; 

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/dbfirst/students")]
    public class DbFirstStudentsController : ControllerBase
    {
        private readonly StudentDbFirstContext _db;
        public DbFirstStudentsController(StudentDbFirstContext db) => _db = db;
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Students.AsNoTracking().ToListAsync();
            if (list.Count == 0) return NotFound(new { message = "No students found (DB-First)." });
            return Ok(list);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            if (id <= 0) return BadRequest(new { message = "Invalid id." });
            var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return s is null ? NotFound(new { message = $"Student {id} not found." }) : Ok(s);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest(new { message = "name is required" });
            var term = name.Trim();
            var results = await _db.Students
                .AsNoTracking()
                .Where(s => EF.Functions.Like(s.Name, $"%{term}%"))
                .ToListAsync();
            return Ok(results);
        }

        public record CreateDto(long Id, string Name, string Email);
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDto dto)
        {
            if (dto.Id <= 0 || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Id, Name, Email are required" });

            var exists = await _db.Students.AnyAsync(s => s.Id == dto.Id || s.Email == dto.Email);
            if (exists) return Conflict(new { message = "Student with same Id or Email exists" });

            var entity = new Student { Id = dto.Id, Name = dto.Name.Trim(), Email = dto.Email.Trim() };
            _db.Students.Add(entity);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        public record UpdateDto(long Id, string Name, string Email);
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDto dto)
        {
            if (dto.Id <= 0 || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Invalid payload" });

            var entity = await _db.Students.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity is null) return NotFound(new { message = $"Student {dto.Id} not found" });

            var emailInUse = await _db.Students.AnyAsync(s => s.Email == dto.Email && s.Id != dto.Id);
            if (emailInUse) return Conflict(new { message = "Email already in use" });

            entity.Name = dto.Name.Trim();
            entity.Email = dto.Email.Trim();
            await _db.SaveChangesAsync();

            return Ok(entity);
        }
        
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id <= 0) return BadRequest(new { message = "Invalid id." });

            var entity = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return NotFound(new { message = $"Student {id} not found" });

            _db.Students.Remove(entity);
            await _db.SaveChangesAsync();
            return Ok(new { message = $"Student {id} deleted (DB-First)." });
        }
    }
}