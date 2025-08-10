using MediatR;
using StudentApi.Models;
using StudentApi.Services;

namespace StudentApi.Features.Students.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Student>
    {
        private readonly IStudentService _studentService;

        public GetStudentByIdQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Task<Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = _studentService.GetById(request.Id);
            return Task.FromResult(student);
        }
    }
}