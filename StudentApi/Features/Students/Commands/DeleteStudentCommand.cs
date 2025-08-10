using MediatR;

namespace StudentApi.Features.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteStudentCommand(int id)
        {
            Id = id;
        }
    }
}