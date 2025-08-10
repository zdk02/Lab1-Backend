using MediatR;
using StudentApi.Models;

namespace StudentApi.Features.Students.Queries
{
    public class SearchStudentsQuery : IRequest<IEnumerable<Student>>
    {
        public string Name { get; }

        public SearchStudentsQuery(string name)
        {
            Name = name;
        }
    }
}