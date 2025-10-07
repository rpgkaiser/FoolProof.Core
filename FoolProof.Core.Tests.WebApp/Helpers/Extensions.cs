using System.Net;

namespace FoolProof.Core.Tests.E2eTests.WebApp
{
    public static class Extensions
    {
        public static bool? UseInputTypes(this HttpRequest request)
        {
            return request.Query.TryGetValue("__useInputTypes__", out var vals)
                    && bool.TryParse(vals.FirstOrDefault(), out var useInputTypes)
                    ? useInputTypes
                    : request.Cookies.TryGetValue("UseInputTypes", out var cookie)
                        && bool.TryParse(cookie, out useInputTypes)
                        ? useInputTypes
                        : (bool?)null;
        }

        public static bool? UseJQuery(this HttpRequest request)
        {
            return request.Query.TryGetValue("__useJQuery__", out var useJQueryStr)
                    && bool.TryParse(useJQueryStr.FirstOrDefault(), out var useJQuery)
                    ? useJQuery
                    : !request.Cookies.TryGetValue("UseJQuery", out var cookie)
                        || !bool.TryParse(cookie, out useJQuery)
                        || useJQuery;
        }
    }
}
