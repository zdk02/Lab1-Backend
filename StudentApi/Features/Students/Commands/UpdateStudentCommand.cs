using MediatR;
using StudentApi.Models;

namespace StudentApi.Features.Students.Commands
{
    public class UpdateStudentCommand : IRequest<Student?>
    {
        public UpdateStudentDto UpdatedStudent { get; }

        public UpdateStudentCommand(UpdateStudentDto updatedStudent)
        {
            UpdatedStudent = updatedStudent;
        }
    }
}