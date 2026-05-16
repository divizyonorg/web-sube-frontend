using System.Text.Json.Serialization;

namespace MyApp.Web.Models.Customers;

public class DestekTalebiDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("konu_basligi")]
    public string KonuBasligi { get; set; } = string.Empty;

    [JsonPropertyName("konu_detaylari")]
    public string? KonuDetaylari { get; set; }

    [JsonPropertyName("customer_id")]
    public int CustomerId { get; set; }

    [JsonPropertyName("gsm")]
    public string? Gsm { get; set; }

    [JsonPropertyName("unit_id")]
    public int? UnitId { get; set; }

    [JsonPropertyName("atanan_birim")]
    public string? AtananBirim { get; set; }

    [JsonPropertyName("durum")]
    public string Durum { get; set; } = string.Empty;

    [JsonPropertyName("destek_tarihi")]
    public DateTime DestekTarihi { get; set; }
}
