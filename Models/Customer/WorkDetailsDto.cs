using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customer;

public class WorkDetailsDto
{
    [JsonPropertyName("work_sector")]
    public int WorkSector { get; set; }

    [JsonPropertyName("work_sector_name")]
    public string WorkSectorName { get; set; } = string.Empty;

    [JsonPropertyName("occupation_id")]
    public int OccupationId { get; set; }

    [JsonPropertyName("occupation_name")]
    public string OccupationName { get; set; } = string.Empty;

    [JsonPropertyName("total_working_time")]
    public string TotalWorkingTime { get; set; } = string.Empty;
}
