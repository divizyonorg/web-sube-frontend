using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.ViewModels;

public class KrediDanismaniViewModel
{
    [Required]
    public string KrediTuru { get; set; } = string.Empty;

    [Required]
    public string KrediTutari { get; set; } = string.Empty;

    [Required]
    public string Vade { get; set; } = string.Empty;

    public string EkBilgi { get; set; } = string.Empty;
}
