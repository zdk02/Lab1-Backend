using MediatR;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<Student>
    {
        public int Id { get; }

        public GetStudentByIdQuery(int id)
        {
            Id = id;
        }
    }
}