using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PhishingDetectorApi.Filters;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-RapidAPI-Key"; // ან შენი სასურველი ჰედერი

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // ამოწმებს მოთხოვნის ჰედერებში არის თუ არა ქი
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing.");
            return;
        }

        string expectedApiKey = "my-super-secret-rapidapi-key"; 

        if (!expectedApiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key.");
        }
    }
}