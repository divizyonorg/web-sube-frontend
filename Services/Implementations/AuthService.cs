using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using MyApp.Web.Models.Auth;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;

    private static class Endpoints
    {
        public const string SendOtp   = "send-otp";
        public const string VerifyOtp = "verify-otp";
    }

    public AuthService(HttpClient httpClient, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _logger     = logger;
    }

    public async Task<(bool Success, string? Message)> SendOtpAsync(string phoneNumber, string tckn)
    {
        var gsm = FormatGsm(phoneNumber);
        _logger.LogInformation("SendOtp → gsm='{Gsm}' tckn='{Tckn}'", gsm, tckn);

        try
        {
            var request  = new SendOtpRequest { Gsm = gsm, Tckn = tckn };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.SendOtp, request);
            var body     = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("SendOtp ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
            {
                var message = TryParseMessage(body);
                return (false, message);
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendOtp exception [{ExType}]: {Message}", ex.GetType().Name, ex.Message);
            return (false, $"{ex.GetType().Name}: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? Message)> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        var gsm = FormatGsm(phoneNumber);
        _logger.LogInformation("VerifyOtp → gsm='{Gsm}'", gsm);

        try
        {
            var request  = new VerifyOtpRequest { Gsm = gsm, OtpCode = otpCode };
            var response = await _httpClient.PostAsJsonAsync(Endpoints.VerifyOtp, request);
            var body     = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("VerifyOtp ← {StatusCode} {Body}", (int)response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
                return (false, TryParseMessage(body));

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VerifyOtp exception [{ExType}]: {Message}", ex.GetType().Name, ex.Message);
            return (false, $"{ex.GetType().Name}: {ex.Message}");
        }
    }

    // "+90 552 265 94 90" → "05522659490"
    private static string FormatGsm(string phoneNumber)
    {
        var digits = Regex.Replace(phoneNumber ?? string.Empty, @"\D", "");
        return digits.Length >= 12 ? "0" + digits[2..] : digits;
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
