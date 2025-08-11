using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class GetAllStudentsQueryHandler
        : IRequestHandler<GetAllStudentsQuery, IEnumerable<Student>>
    {
        private readonly StudentDbContext _db;

        public GetAllStudentsQueryHandler(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Student>> Handle(
            GetAllStudentsQuery request,
            CancellationToken cancellationToken)
        {
            return await _db.Students
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}