using MediatR;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<Student>
    {
        public long Id { get; }

        public GetStudentByIdQuery(long id)
        {
            Id = id;
        }
    }
}