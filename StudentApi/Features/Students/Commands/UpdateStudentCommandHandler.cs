using MediatR;
using StudentApi.Models;
using StudentApi.Services;

namespace StudentApi.Features.Students.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Student?>
    {
        private readonly IStudentService _studentService;

        public UpdateStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Task<Student?> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var result = _studentService.Update(request.UpdatedStudent);
            return Task.FromResult(result);
        }
    }
}