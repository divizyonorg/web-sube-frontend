using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class DynamicDataResponseDto<TDetails>
{
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<DynamicDataItemDto<TDetails>> Data { get; set; } = [];

    [JsonPropertyName("pagination")]
    public PaginationDto Pagination { get; set; } = new();
}

public class DynamicDataItemDto<TDetails>
{
    [JsonPropertyName("module_type")]
    public string ModuleType { get; set; } = string.Empty;

    [JsonPropertyName("create_date")]
    public DateTime CreateDate { get; set; }

    [JsonPropertyName("details")]
    public TDetails Details { get; set; } = default!;
}

public class PaginationDto
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
