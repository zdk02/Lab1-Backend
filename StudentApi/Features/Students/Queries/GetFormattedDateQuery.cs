using MediatR;

namespace StudentApi.Features.Students.Queries
{
    public class GetFormattedDateQuery : IRequest<GetFormattedDateResult>
    {
        public string AcceptLanguage { get; set; } = string.Empty;
    }
    public class GetFormattedDateResult
    {
        public string Date { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
    }
}