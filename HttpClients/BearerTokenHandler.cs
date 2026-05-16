using System.Net.Http.Headers;

namespace MyApp.Web.HttpClients;

public class BearerTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;

    public BearerTokenHandler(IHttpContextAccessor accessor) => _accessor = accessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null)
            return base.SendAsync(request, cancellationToken);

        var context = _accessor.HttpContext;

        string? token = null;

        var authHeader = context?.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = authHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(token))
            token = context?.Request.Cookies["auth_token"];

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
