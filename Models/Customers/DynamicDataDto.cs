using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class DynamicDataResponseDto
{
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<DynamicDataItemDto> Data { get; set; } = [];

    [JsonPropertyName("pagination")]
    public DynamicDataPaginationDto Pagination { get; set; } = new();
}

public class DynamicDataItemDto
{
    [JsonPropertyName("module_type")]
    public string ModuleType { get; set; } = string.Empty;

    [JsonPropertyName("create_date")]
    public string CreateDate { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public JsonElement? Details { get; set; }
}

public class DynamicDataPaginationDto
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("has_next_page")]
    public bool HasNextPage { get; set; }
}

public class FullnameDetailsDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }

    [JsonPropertyName("tckn")]
    public string? Tckn { get; set; }
}

public class KvkkDetailsDto
{
    [JsonPropertyName("channel_id")]
    public int ChannelId { get; set; }

    [JsonPropertyName("permissions")]
    public KvkkPermissionsDto? Permissions { get; set; }
}

public class KvkkPermissionsDto
{
    [JsonPropertyName("call")]
    public bool Call { get; set; }

    [JsonPropertyName("email")]
    public bool Email { get; set; }

    [JsonPropertyName("sms")]
    public bool Sms { get; set; }

    [JsonPropertyName("adress")]
    public bool Adress { get; set; }
}

public class ContactDetailsDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("is_primary")]
    public bool IsPrimary { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}

public class MartialStatusDetailsDto
{
    [JsonPropertyName("marital_status")]
    public bool MaritalStatus { get; set; }

    [JsonPropertyName("is_working")]
    public bool? IsWorking { get; set; }

    [JsonPropertyName("w_salary_amount")]
    public string? WSalaryAmount { get; set; }
}
