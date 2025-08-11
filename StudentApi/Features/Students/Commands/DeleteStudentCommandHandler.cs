using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;

namespace StudentApi.Features.Students.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly StudentDbContext _db;

        public DeleteStudentCommandHandler(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Students
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (entity is null) return false;

            _db.Students.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}