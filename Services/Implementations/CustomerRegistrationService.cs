using System.Net.Http.Json;
using System.Text.Json;
using MyApp.Web.Models.Register;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Services.Implementations;

public class CustomerRegistrationService : ICustomerRegistrationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerRegistrationService> _logger;

    private static class Endpoints
    {
        public const string Create = "api/customers/create";
        public const string Kvkk = "api/customers/kvkk";
        public const string Contact = "api/customers/contact";
    }

    public CustomerRegistrationService(HttpClient httpClient, ILogger<CustomerRegistrationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(bool Success, string? Message, string? NewToken)> CreateCustomerAsync(CreateCustomerRequest request)
    {
        _logger.LogInformation("CreateCustomer → tckn='{Tckn}' gsm='{Gsm}'", request.Tckn, request.Gsm);

        try
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoints.Create, request);
            var body = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("CreateCustomer ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
                return (false, TryParseMessage(body), null);

            string? newToken = null;
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("new_access_token", out var t))
                    newToken = t.GetString();
            }
            catch { }

            return (true, null, newToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateCustomer exception: {Message}", ex.Message);
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? Message)> UpdateKvkkAsync(KvkkRequest request, string? bearerToken = null)
    {
        _logger.LogInformation("UpdateKvkk → channel_id={ChannelId}", request.ChannelId);

        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoints.Kvkk)
            {
                Content = JsonContent.Create(request)
            };

            if (!string.IsNullOrWhiteSpace(bearerToken))
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(httpRequest);
            var body = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("UpdateKvkk ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
                return (false, TryParseMessage(body));

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateKvkk exception: {Message}", ex.Message);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Message)> UpdateContactAsync(ContactRequest request, string? bearerToken = null)
    {
        _logger.LogInformation("UpdateContact → email='{Email}'", request.Email);

        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoints.Contact)
            {
                Content = JsonContent.Create(request)
            };

            if (!string.IsNullOrWhiteSpace(bearerToken))
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(httpRequest);
            var body = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("UpdateContact ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
                return (false, TryParseMessage(body));

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateContact exception: {Message}", ex.Message);
            return (false, ex.Message);
        }
    }

    private static string? TryParseMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("message", out var msg))
                return msg.GetString();
        }
        catch { }
        return null;
    }
}
