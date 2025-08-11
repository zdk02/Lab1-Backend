using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Features.Students.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Student?>
    {
        private readonly StudentDbContext _db;

        public UpdateStudentCommandHandler(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<Student?> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdatedStudent;
            
            var entity = await _db.Students.FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);
            if (entity is null) return null;

            var newName = dto.Name?.Trim() ?? string.Empty;
            var newEmail = dto.Email?.Trim() ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail))
                return null;

            var emailExists = await _db.Students
                .AnyAsync(s => s.Email == newEmail && s.Id != dto.Id, cancellationToken);
            if (emailExists)
            {

                throw new System.ArgumentException("Email is already in use by another student.");
            }
            
            entity.Name = newName;
            entity.Email = newEmail;

            await _db.SaveChangesAsync(cancellationToken);

            return entity; 
        }
    }
}