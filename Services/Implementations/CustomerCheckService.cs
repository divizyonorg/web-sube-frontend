using System.Text.Json;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Services.Implementations;

public class CustomerCheckService : ICustomerCheckService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerCheckService> _logger;

    private static class Endpoints
    {
        public const string CheckTcknMatch = "/api/customers/check-tckn-match/{0}";
    }

    public CustomerCheckService(HttpClient httpClient, ILogger<CustomerCheckService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> CheckTcknMatchAsync(string tckn, string? bearerToken = null, CancellationToken ct = default)
    {
        _logger.LogInformation("CheckTcknMatch → tckn='{Tckn}'", tckn);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, string.Format(Endpoints.CheckTcknMatch, tckn));
            if (!string.IsNullOrWhiteSpace(bearerToken))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("CheckTcknMatch ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode) return false;

            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("is_match", out var match))
            {
                if (match.ValueKind == JsonValueKind.True) return true;
                if (match.ValueKind == JsonValueKind.False) return false;
                if (match.ValueKind == JsonValueKind.Number) return match.GetInt32() == 1;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CheckTcknMatch exception [{ExType}]: {Message}", ex.GetType().Name, ex.Message);
        }

        return false;
    }
}
