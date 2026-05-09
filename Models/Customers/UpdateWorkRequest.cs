using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class UpdateWorkRequest
{
    [JsonPropertyName("work_sector")]
    public int WorkSector { get; set; }

    [JsonPropertyName("occupation_id")]
    public int OccupationId { get; set; }

    [JsonPropertyName("total_working_time")]
    public string TotalWorkingTime { get; set; } = string.Empty;

    [JsonPropertyName("app")]
    public string App { get; set; } = string.Empty;
}
