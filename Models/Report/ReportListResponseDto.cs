using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class ReportListResponseDto
{
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<ReportListItemDto> Data { get; set; } = [];

    [JsonPropertyName("pagination")]
    public ReportListPaginationDto? Pagination { get; set; }
}

public class ReportListItemDto
{
    [JsonPropertyName("report_id")]
    public int ReportId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;

    [JsonPropertyName("cid")]
    public int Cid { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("create_date")]
    public DateTime CreateDate { get; set; }

    [JsonPropertyName("update_date")]
    public DateTime UpdateDate { get; set; }
}

public class ReportListPaginationDto
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
