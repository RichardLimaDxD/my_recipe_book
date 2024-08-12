using System.Globalization;

namespace MyRepiceBook.API.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            CultureInfo[] supportedLangueges = CultureInfo.GetCultures(CultureTypes.AllCultures);

            string? requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            CultureInfo? cultureInfo = new CultureInfo("en");

            if (!string.IsNullOrWhiteSpace(requestedCulture) &&
                supportedLangueges.Any(culture => culture.Name.Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
