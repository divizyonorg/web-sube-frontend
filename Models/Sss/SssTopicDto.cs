using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Sss;

public class SssTopicDto
{
    [JsonPropertyName("topic_id")]
    public int TopicId { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("parent_topic_id")]
    public int? ParentTopicId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("answer")]
    public string? Answer { get; set; }
}
