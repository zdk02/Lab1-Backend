using MediatR;
using StudentApi.Models;
using StudentApi.Services;

namespace StudentApi.Features.Students.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<Student>>
    {
        private readonly IStudentService _studentService;

        public GetAllStudentsQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Task<IEnumerable<Student>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var result = _studentService.GetAll();
            return Task.FromResult(result);
        }
    }
}