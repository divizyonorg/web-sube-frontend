using System.Text.Json;
using System.Text.RegularExpressions;
using MyApp.Web.HttpClients;
using MyApp.Web.Models.Customers;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;
using MyApp.Web.ViewModels.Components;

namespace MyApp.Web.Services.Implementations;

public class CustomerDataService : ICustomerDataService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomerDataService> _logger;

    private static class Endpoints
    {
        public const string Category = "/api/customers/{0}";
        public const string Contact = "/api/customers/contact";
        public const string FindeksGsm = "/api/customers/findeks/update-gsm";
        public const string Kvkk = "/api/customers/kvkk";
        public const string MaritalStatus = "/api/customers/marital-status";
        public const string Work = "/api/customers/work";

        public static string DynamicData(string moduleType, int page = 1)
            => $"/api/customers/dynamic-data?module_type={moduleType}&page={page}";
    }

    public CustomerDataService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<CustomerDataService> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private void AttachToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["auth_token"];
        if (string.IsNullOrWhiteSpace(token)) return;

        _httpClient.DefaultRequestHeaders.Remove("Authorization");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    private async Task<List<SelectOption>> GetCategoryAsync(string category, CancellationToken cancellationToken)
    {
        AttachToken();
        var response = await ApiClient.GetJsonAsync<CategoryListResponseDto>(
            _httpClient,
            string.Format(Endpoints.Category, category),
            cancellationToken);

        return response?.Data.Select(d => new SelectOption { Value = d.Value, Label = d.Label }).ToList() ?? [];
    }

    public Task<List<SelectOption>> GetWorkSectorsAsync(CancellationToken cancellationToken = default)
        => GetCategoryAsync("work_sectors", cancellationToken);

    public Task<List<SelectOption>> GetOccupationsAsync(CancellationToken cancellationToken = default)
        => GetCategoryAsync("occupations", cancellationToken);

    public Task<List<SelectOption>> GetBanksAsync(CancellationToken cancellationToken = default)
        => GetCategoryAsync("banks", cancellationToken);

    public async Task<ProfilBilgileriViewModel> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        AttachToken();

        var fullnameTask = ApiClient.GetJsonAsync<DynamicDataResponseDto>(_httpClient, Endpoints.DynamicData("FULLNAME"), cancellationToken);
        var contactTask = ApiClient.GetJsonAsync<DynamicDataResponseDto>(_httpClient, Endpoints.DynamicData("CONTACT"), cancellationToken);

        await Task.WhenAll(fullnameTask, contactTask);

        var vm = new ProfilBilgileriViewModel();

        _logger.LogInformation("FULLNAME raw: {Json}", System.Text.Json.JsonSerializer.Serialize(fullnameTask.Result));
        _logger.LogInformation("CONTACT raw: {Json}", System.Text.Json.JsonSerializer.Serialize(contactTask.Result));

        var fullnameItem = fullnameTask.Result?.Data.FirstOrDefault();
        if (fullnameItem is not null && fullnameItem.Details.HasValue)
        {
            var details = fullnameItem.Details.Value.Deserialize<FullnameDetailsDto>();
            _logger.LogInformation("FULLNAME details → FirstName='{First}' LastName='{Last}' Birthday='{Birthday}' Tckn='{Tckn}'",
                details?.FirstName, details?.LastName, details?.Birthday, details?.Tckn);
            if (details is not null)
            {
                vm.FullName = $"{details.FirstName} {details.LastName}".Trim();
                vm.Birthday = details.Birthday ?? string.Empty;
                vm.Tckn = details.Tckn ?? string.Empty;
            }
        }
        else
        {
            _logger.LogWarning("FULLNAME: veri yok veya details boş. Item null={IsNull}", fullnameItem is null);
        }

        foreach (var item in contactTask.Result?.Data ?? [])
        {
            if (!item.Details.HasValue) continue;
            var details = item.Details.Value.Deserialize<ContactDetailsDto>();
            _logger.LogInformation("CONTACT item → Type='{Type}' Value='{Value}' IsPrimary={IsPrimary}", details?.Type, details?.Value, details?.IsPrimary);
            if (details is null || !details.IsPrimary) continue;

            if (details.Type.Equals("GSM", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(vm.Phone)) vm.Phone = details.Value;
            if (details.Type.Equals("Email", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(vm.Email)) vm.Email = details.Value;
        }

        _logger.LogInformation("GetProfile result → FullName='{FullName}' Email='{Email}' Phone='{Phone}'", vm.FullName, vm.Email, vm.Phone);

        return vm;
    }

    public async Task<bool> UpdateContactAsync(string gsm, string email, CancellationToken cancellationToken = default)
    {
        AttachToken();
        var request = new UpdateContactRequest { Gsm = gsm, Email = email };
        return await ApiClient.PostJsonAsync(_httpClient, Endpoints.Contact, request, cancellationToken);
    }

    public async Task<BildirimlerViewModel> GetKvkkAsync(CancellationToken cancellationToken = default)
    {
        AttachToken();
        var response = await ApiClient.GetJsonAsync<DynamicDataResponseDto>(_httpClient, Endpoints.DynamicData("KVKK"), cancellationToken);
        var item = response?.Data.FirstOrDefault();
        if (item is null || !item.Details.HasValue) return new BildirimlerViewModel();

        var details = item.Details.Value.Deserialize<KvkkDetailsDto>();
        if (details is null) return new BildirimlerViewModel();

        return new BildirimlerViewModel
        {
            ChannelId = details.ChannelId,
            Email = details.Permissions?.Email ?? false,
            Sms = details.Permissions?.Sms ?? false,
            Call = details.Permissions?.Call ?? false,
            Adress = details.Permissions?.Adress ?? false
        };
    }

    public async Task<bool> UpdateKvkkAsync(int channelId, bool email, bool sms, bool call, bool adress, CancellationToken cancellationToken = default)
    {
        AttachToken();
        var request = new UpdateKvkkRequest
        {
            ChannelId = channelId,
            Email = email,
            Sms = sms,
            Call = call,
            Adress = adress
        };
        return await ApiClient.PostJsonAsync(_httpClient, Endpoints.Kvkk, request, cancellationToken);
    }

    public async Task<MaritalStatusViewModel> GetMaritalStatusAsync(CancellationToken cancellationToken = default)
    {
        AttachToken();
        var response = await ApiClient.GetJsonAsync<DynamicDataResponseDto>(
            _httpClient, Endpoints.DynamicData("MARITAL_STATUS"), cancellationToken);

        var item = response?.Data.FirstOrDefault();
        if (item is null || !item.Details.HasValue) return new MaritalStatusViewModel();

        var details = item.Details.Value.Deserialize<MartialStatusDetailsDto>();
        if (details is null) return new MaritalStatusViewModel();

        return new MaritalStatusViewModel
        {
            IsMarried = details.MaritalStatus,
            IsWorking = details.IsWorking ?? false,
            WSalaryAmount = decimal.TryParse(details.WSalaryAmount, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var amount) ? amount : 0
        };
    }

    public async Task<bool> UpdateMaritalStatusAsync(bool maritalStatus, bool isWorking, decimal wSalaryAmount, CancellationToken cancellationToken = default)
    {
        AttachToken();
        var request = new UpdateMaritalStatusRequest
        {
            MaritalStatus = maritalStatus,
            IsWorking = maritalStatus && isWorking,
            WSalaryAmount = maritalStatus && isWorking ? wSalaryAmount : 0
        };
        return await ApiClient.PostJsonAsync(_httpClient, Endpoints.MaritalStatus, request, cancellationToken);
    }

    public async Task<bool> UpdateWorkAsync(int workSector, int occupationId, string totalWorkingTime, CancellationToken cancellationToken = default)
    {
        AttachToken();
        var request = new UpdateWorkRequest
        {
            WorkSector = workSector,
            OccupationId = occupationId,
            TotalWorkingTime = totalWorkingTime
        };
        return await ApiClient.PostJsonAsync(_httpClient, Endpoints.Work, request, cancellationToken);
    }

    public async Task<bool> GetWorkStatusAsync(CancellationToken cancellationToken = default)
    {
        var (workSector, _, _) = await GetWorkDetailsAsync(cancellationToken);
        return workSector > 0;
    }

    public async Task<(int WorkSector, int OccupationId, string TotalWorkingTime)> GetWorkDetailsAsync(CancellationToken cancellationToken = default)
    {
        AttachToken();
        var response = await ApiClient.GetJsonAsync<DynamicDataResponseDto>(
            _httpClient, Endpoints.DynamicData("WORK"), cancellationToken);

        var item = response?.Data.OrderByDescending(d => d.CreateDate).FirstOrDefault();
        if (item is null || !item.Details.HasValue) return (0, 0, string.Empty);

        var details = item.Details.Value;
        var workSector = details.TryGetProperty("work_sector", out var ws) ? ws.GetInt32() : 0;
        var occupationId = details.TryGetProperty("occupation_id", out var oc) ? oc.GetInt32() : 0;
        var workingTime = details.TryGetProperty("total_working_time", out var wt) ? wt.GetString() ?? "" : "";
        return (workSector, occupationId, workingTime);
    }

    public async Task<string> GetSalaryBankCodeAsync(CancellationToken cancellationToken = default)
    {
        AttachToken();
        var response = await ApiClient.GetJsonAsync<DynamicDataResponseDto>(
            _httpClient, Endpoints.DynamicData("SALARY"), cancellationToken);

        var item = response?.Data.OrderByDescending(d => d.CreateDate).FirstOrDefault();
        if (item is null || !item.Details.HasValue) return string.Empty;

        return item.Details.Value.TryGetProperty("salary_bank_eft_code", out var bc) ? bc.GetString() ?? "" : "";
    }

    public async Task<bool> UpdateGsmAsync(string gsm, CancellationToken cancellationToken = default)
    {
        AttachToken();
        var normalized = Regex.Replace(gsm ?? "", @"\D", "");
        var request = new UpdateGsmRequest { Gsm = normalized };
        return await ApiClient.PostJsonAsync(_httpClient, Endpoints.FindeksGsm, request, cancellationToken);
    }

}
