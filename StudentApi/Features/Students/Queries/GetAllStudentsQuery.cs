using MediatR;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class GetAllStudentsQuery : IRequest<IEnumerable<Student>>
    {
    }
}