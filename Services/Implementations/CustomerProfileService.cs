using System.Text.RegularExpressions;
using MyApp.Web.Models.Customer;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class CustomerProfileService : ICustomerProfileService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerProfileService> _logger;

    private static class Endpoints
    {
        public const string DynamicData = "/api/customers/dynamic-data";
        public const string FullName = $"{DynamicData}?module_type=FULLNAME&page=1";
        public const string Contact = $"{DynamicData}?module_type=CONTACT&page=1";
    }

    public CustomerProfileService(HttpClient httpClient, ILogger<CustomerProfileService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CustomerProfileViewModel?> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var fullNameTask = FetchAsync<FullNameDetailsDto>(Endpoints.FullName, cancellationToken);
        var contactTask = FetchAsync<ContactDetailsDto>(Endpoints.Contact, cancellationToken);

        await Task.WhenAll(fullNameTask, contactTask);

        var fullName = fullNameTask.Result?.Data.FirstOrDefault()?.Details;
        if (fullName is null)
        {
            _logger.LogWarning("FULLNAME verisi alınamadı.");
            return null;
        }

        var gsmEntries = contactTask.Result?.Data
            .Where(d => d.Details.Type.Equals("GSM", StringComparison.OrdinalIgnoreCase))
            .ToList();
        var contact = gsmEntries?
            .FirstOrDefault(d => d.Details.IsPrimary && d.Details.IsActive)?.Details
            ?? gsmEntries?.FirstOrDefault()?.Details;

        return MapToViewModel(fullName, contact);
    }

    private async Task<DynamicDataResponseDto<TDetails>?> FetchAsync<TDetails>(string endpoint, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("GET {Endpoint} → {Status}", endpoint, (int)response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} başarısız: {Status} — {Body}", endpoint, (int)response.StatusCode, body);
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<DynamicDataResponseDto<TDetails>>(body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} exception: {Message}", endpoint, ex.Message);
            return null;
        }
    }

    private static CustomerProfileViewModel MapToViewModel(FullNameDetailsDto fullName, ContactDetailsDto? contact) => new()
    {
        FullName = $"{fullName.FirstName} {fullName.LastName}",
        PhoneNumber = FormatPhone(contact?.Value),
        Birthday = FormatBirthday(fullName.Birthday),
        Tckn = fullName.Tckn
    };

    private static string FormatPhone(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var digits = Regex.Replace(value, @"\D", "");
        return digits.Length == 10
            ? $"0{digits[..3]} {digits[3..6]} {digits[6..8]} {digits[8..10]}"
            : value;
    }

    private static string FormatBirthday(string value)
        => DateTime.TryParse(value, out var date) ? date.ToString("dd.MM.yyyy") : value;
}
