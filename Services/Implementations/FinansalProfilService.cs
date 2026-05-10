using System.Net.Http.Json;
using System.Text.Json;
using MyApp.Web.Models.Customer;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;

namespace MyApp.Web.Services.Implementations;

public class FinansalProfilService : IFinansalProfilService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FinansalProfilService> _logger;

    private static class Endpoints
    {
        public const string WorkSectors  = "/api/customers/work_sectors";
        public const string Occupations  = "/api/customers/occupations";
        public const string DynamicData  = "/api/customers/dynamic-data";
        public const string SaveWork     = "/api/customers/work";
        public const string SaveSalary   = "/api/customers/salary";
        public const string SaveMarital  = "/api/customers/marital-status";
        public const string SaveHavings  = "/api/customers/havings";

        public static string Dynamic(string moduleType) =>
            $"{DynamicData}?module_type={moduleType}&page=1";
    }

    public FinansalProfilService(HttpClient httpClient, ILogger<FinansalProfilService> logger)
    {
        _httpClient = httpClient;
        _logger     = logger;
    }

    public async Task<FinansalProfilViewModel> GetAsync(CancellationToken ct = default)
    {
        var workSectorsTask  = GetLookupAsync(Endpoints.WorkSectors, ct);
        var occupationsTask  = GetLookupAsync(Endpoints.Occupations, ct);
        var workDataTask     = GetDynamicAsync<WorkDetailsDto>("WORK", ct);
        var salaryDataTask   = GetDynamicAsync<SalaryDetailsDto>("SALARY", ct);
        var maritalDataTask  = GetDynamicAsync<MaritalStatusDetailsDto>("MARITAL_STATUS", ct);
        var havingsDataTask  = GetDynamicAsync<HavingsDetailsDto>("HAVINGS", ct);

        await Task.WhenAll(workSectorsTask, occupationsTask,
                           workDataTask, salaryDataTask, maritalDataTask, havingsDataTask);

        var work    = workDataTask.Result?.Data.MaxBy(d => d.CreateDate)?.Details;
        var salary  = salaryDataTask.Result?.Data.MaxBy(d => d.CreateDate)?.Details;
        var marital = maritalDataTask.Result?.Data.MaxBy(d => d.CreateDate)?.Details;
        var havings = havingsDataTask.Result?.Data.MaxBy(d => d.CreateDate)?.Details;

        return new FinansalProfilViewModel
        {
            WorkSectors      = MapLookup(workSectorsTask.Result),
            Occupations      = MapLookup(occupationsTask.Result),
            WorkSectorId     = work?.WorkSector ?? 0,
            OccupationId     = work?.OccupationId ?? 0,
            TotalWorkingTime = work?.TotalWorkingTime ?? string.Empty,
            SalaryAmount     = decimal.TryParse(salary?.CustSalaryAmount,
                                   System.Globalization.NumberStyles.Any,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out var amt) ? amt : 0,
            IsMarried        = marital?.MaritalStatus ?? false,
            HouseStatusId    = MapHouseStatus(havings?.HouseStatusName),
            HasCar           = havings?.CarStatus ?? false,
        };
    }

    public async Task<(bool Success, string Message)> SaveWorkAsync(
        int workSectorId, int occupationId, string totalWorkingTime, CancellationToken ct = default)
    {
        var req = new SaveWorkRequest
        {
            WorkSector       = workSectorId,
            OccupationId     = occupationId,
            TotalWorkingTime = totalWorkingTime
        };
        return await PostAsync(Endpoints.SaveWork, req, ct);
    }

    public async Task<(bool Success, string Message)> SaveSalaryAsync(decimal salaryAmount, CancellationToken ct = default)
    {
        var req = new SaveSalaryRequest { SalaryAmount = salaryAmount };
        return await PostAsync(Endpoints.SaveSalary, req, ct);
    }

    public async Task<(bool Success, string Message)> SaveMaritalStatusAsync(bool isMarried, CancellationToken ct = default)
    {
        var req = new SaveMaritalStatusRequest { MaritalStatus = isMarried };
        return await PostAsync(Endpoints.SaveMarital, req, ct);
    }

    public async Task<(bool Success, string Message)> SaveHavingsAsync(int houseStatusId, bool hasCar, CancellationToken ct = default)
    {
        var req = new SaveHavingsRequest { HouseStatus = houseStatusId, CarStatus = hasCar };
        return await PostAsync(Endpoints.SaveHavings, req, ct);
    }

    // ── private helpers ────────────────────────────────────────────

    private async Task<LookupResponseDto?> GetLookupAsync(string endpoint, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("GET {Endpoint} → {Status}", endpoint, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} başarısız: {Body}", endpoint, body);
                return null;
            }
            return JsonSerializer.Deserialize<LookupResponseDto>(body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} exception", endpoint);
            return null;
        }
    }

    private async Task<DynamicDataResponseDto<TDetails>?> GetDynamicAsync<TDetails>(string moduleType, CancellationToken ct)
    {
        var endpoint = Endpoints.Dynamic(moduleType);
        try
        {
            var response = await _httpClient.GetAsync(endpoint, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("GET {Endpoint} → {Status}", endpoint, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} başarısız: {Body}", endpoint, body);
                return null;
            }
            return JsonSerializer.Deserialize<DynamicDataResponseDto<TDetails>>(body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} exception", endpoint);
            return null;
        }
    }

    private async Task<(bool Success, string Message)> PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, request, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);
            _logger.LogInformation("POST {Endpoint} → {Status}", endpoint, (int)response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("POST {Endpoint} başarısız: {Body}", endpoint, body);
                var errMsg = TryParseMessage(body) ?? "Kayıt sırasında hata oluştu.";
                return (false, errMsg);
            }
            return (true, "Bilgiler başarıyla kaydedildi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} exception", endpoint);
            return (false, "Bağlantı hatası oluştu.");
        }
    }

    private static List<LookupItemViewModel> MapLookup(LookupResponseDto? dto)
        => dto?.Data.Select(d => new LookupItemViewModel(d.Value, d.Label)).ToList() ?? [];

    private static int MapHouseStatus(string? name) => name?.ToLowerInvariant() switch
    {
        { } n when n.Contains("kendi") || n.Contains("sahib") => 0,
        { } n when n.Contains("kiracı") || n.Contains("kiraci") => 1,
        _ => 2
    };

    private static string? TryParseMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("message", out var m)) return m.GetString();
            if (doc.RootElement.TryGetProperty("detail",  out var d)) return d.GetString();
        }
        catch { }
        return null;
    }
}
