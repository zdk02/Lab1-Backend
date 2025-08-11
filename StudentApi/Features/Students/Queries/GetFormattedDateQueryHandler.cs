using MediatR;
using System.Globalization;

namespace StudentApi.Features.Students.Queries
{
    public class GetFormattedDateQueryHandler 
        : IRequestHandler<GetFormattedDateQuery, GetFormattedDateResult>
    {
        public Task<GetFormattedDateResult> Handle(
            GetFormattedDateQuery request, 
            CancellationToken cancellationToken)
        {
            string culture = "en-US";

            if (!string.IsNullOrWhiteSpace(request.AcceptLanguage))
            {
                var selectedCulture = request.AcceptLanguage.Split(',')[0].Trim();
                var validCultures = CultureInfo
                    .GetCultures(CultureTypes.AllCultures)
                    .Select(c => c.Name)
                    .ToList();

                if (!validCultures.Contains(selectedCulture))
                    throw new CultureNotFoundException(
                        $"Culture '{selectedCulture}' is not supported."
                    );

                culture = selectedCulture;
            }

            var formattedDate = DateTime.Now.ToString("D", new CultureInfo(culture));

            return Task.FromResult(new GetFormattedDateResult
            {
                Date = formattedDate,
                Culture = culture
            });
        }
    }
}