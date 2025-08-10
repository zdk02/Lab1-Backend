using MediatR;
using StudentApi.Services;

namespace StudentApi.Features.Students.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public DeleteStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var result = _studentService.Delete(request.Id);
            return Task.FromResult(result);
        }
    }
}