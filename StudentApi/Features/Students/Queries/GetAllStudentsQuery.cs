using MediatR;
using StudentApi.Models;
using System.Collections.Generic;

namespace StudentApi.Features.Students.Queries
{
    public class GetAllStudentsQuery : IRequest<IEnumerable<Student>>
    {
    }
}