using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class SearchStudentsQueryHandler
        : IRequestHandler<SearchStudentsQuery, IEnumerable<Student>>
    {
        private readonly StudentDbContext _db;

        public SearchStudentsQueryHandler(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Student>> Handle(
            SearchStudentsQuery request,
            CancellationToken cancellationToken)
        {
            var term = request.Name.Trim();

            // Case-insensitive contains using EF.Functions.Like (translates well to SQL)
            return await _db.Students
                .AsNoTracking()
                .Where(s => EF.Functions.Like(s.Name, $"%{term}%"))
                .ToListAsync(cancellationToken);
        }
    }
}