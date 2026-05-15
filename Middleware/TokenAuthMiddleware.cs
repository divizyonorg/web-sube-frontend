using System.Text;
using System.Text.Json;

namespace MyApp.Web.Middleware;

public class TokenAuthMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly string[] _publicPrefixes =
    [
        "/login",
        "/register",
        "/error",
        "/health",
        "/privacy"
    ];

    public TokenAuthMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "/";

        if (_publicPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Cookies["auth_token"];

        if (string.IsNullOrEmpty(token) || IsTokenExpired(token))
        {
            context.Response.Cookies.Delete("auth_token");
            RedirectToLogin(context);
            return;
        }

        await _next(context);
    }

    private static void RedirectToLogin(HttpContext context)
    {
        // HTMX isteklerinde tarayıcı yönlendirmesi için HX-Redirect header kullan
        if (context.Request.Headers.ContainsKey("HX-Request"))
        {
            context.Response.Headers["HX-Redirect"] = "/login";
            context.Response.StatusCode = StatusCodes.Status200OK;
        }
        else
        {
            context.Response.Redirect("/login");
        }
    }

    private static bool IsTokenExpired(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return true;

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');
            payload = payload.PadRight((payload.Length + 3) & ~3, '=');

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("exp", out var exp))
                return exp.GetInt64() < DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return false;
        }
        catch
        {
            return true;
        }
    }
}
