using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Authenticaion;
public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration) =>
        _configuration = configuration;


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API key not found");
            return;
        }
        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
        if (!apiKey.Equals(potentialApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API key not valid");
            return;
        }
    }
}