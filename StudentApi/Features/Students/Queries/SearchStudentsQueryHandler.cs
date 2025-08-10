using MediatR;
using StudentApi.Models;
using StudentApi.Services;

namespace StudentApi.Features.Students.Queries
{
    public class SearchStudentsQueryHandler : IRequestHandler<SearchStudentsQuery, IEnumerable<Student>>
    {
        private readonly IStudentService _studentService;

        public SearchStudentsQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Task<IEnumerable<Student>> Handle(SearchStudentsQuery request, CancellationToken cancellationToken)
        {
            var results = _studentService.Search(request.Name);
            return Task.FromResult(results);
        }
    }
}