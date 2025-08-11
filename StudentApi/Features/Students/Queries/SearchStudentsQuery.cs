using MediatR;
using StudentApi.Models;
using System.Collections.Generic;

namespace StudentApi.Features.Students.Queries
{
    public record SearchStudentsQuery(string Name) : IRequest<IEnumerable<Student>>;
}
