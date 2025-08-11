using MediatR;

namespace StudentApi.Features.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public long Id { get; }

        public DeleteStudentCommand(long id)
        {
            Id = id;
        }
    }
}