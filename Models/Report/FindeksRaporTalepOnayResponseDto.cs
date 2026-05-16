using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Report;

public class FindeksRaporTalepOnayResponseDto
{
    [JsonPropertyName("basari")]
    public bool Basari { get; set; }

    [JsonPropertyName("aksiyon")]
    public string Aksiyon { get; set; } = string.Empty;

    [JsonPropertyName("mesaj")]
    public string Mesaj { get; set; } = string.Empty;

    [JsonPropertyName("talepId")]
    public string TalepId { get; set; } = string.Empty;

    [JsonPropertyName("rid")]
    public string Rid { get; set; } = string.Empty;
}
