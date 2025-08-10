using MediatR;

namespace StudentApi.Features.Students.Queries
{
    public class GetFormattedDateQuery : IRequest<GetFormattedDateResult>
    {
        public string AcceptLanguage { get; set; }
    }
    public class GetFormattedDateResult
    {
        public string Date { get; set; }
        public string Culture { get; set; }
    }
}