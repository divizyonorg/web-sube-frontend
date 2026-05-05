namespace MyApp.Web.ViewModels;

public class KrediRaporlariViewModel
{
    public int TotalCount      { get; set; }
    public int ReadyCount      { get; set; }
    public int ProcessingCount { get; set; }
    public List<ReportItemViewModel> Reports { get; set; } = [];
}

public class ReportItemViewModel
{
    public string Title      { get; set; } = string.Empty;
    public string ReportNo   { get; set; } = string.Empty;
    public string Status     { get; set; } = string.Empty;
    public string Date       { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;

    public bool IsReady       => Status == "Hazır";
    public bool IsProcessing  => Status == "İşleniyor";
    public bool IsPending     => Status == "Beklemede";

    public string StatusBgClass => Status switch
    {
        "Hazır"     => "bg-[#DCFCE7] text-[#008236]",
        "İşleniyor" => "bg-[#FEF9C2] text-[#A65F00]",
        _           => "bg-[#F3F4F6] text-[#4A5565]"
    };
}
