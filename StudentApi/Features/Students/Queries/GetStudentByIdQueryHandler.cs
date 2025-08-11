using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class GetStudentByIdQueryHandler
        : IRequestHandler<GetStudentByIdQuery, Student?>
    {
        private readonly StudentDbContext _db;

        public GetStudentByIdQueryHandler(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        }
    }
}