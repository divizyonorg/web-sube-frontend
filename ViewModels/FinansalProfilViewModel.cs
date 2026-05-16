namespace MyApp.Web.ViewModels;

public class FinansalProfilViewModel
{
    // Lookup listeler (select seçenekleri)
    public List<LookupItemViewModel> WorkSectors { get; set; } = [];
    public List<LookupItemViewModel> Occupations { get; set; } = [];

    // İş bilgileri
    public int WorkSectorId { get; set; }
    public int OccupationId { get; set; }
    public string TotalWorkingTime { get; set; } = string.Empty;

    // Çalışma durumu
    public bool IsEmployed { get; set; }

    // Gelir
    public decimal SalaryAmount { get; set; }

    // Maaş bankası
    public string SalaryBankCode { get; set; } = string.Empty;
    public List<LookupItemViewModel> SalaryBanks { get; set; } = [];

    // Medeni durum
    public bool IsMarried { get; set; }

    // Mülkiyet — 0=Kendi Evim, 1=Kira, 2=Aile Evi
    public int HouseStatusId { get; set; }
    public bool HasCar { get; set; }

    public static readonly List<LookupItemViewModel> WorkingTimeOptions =
    [
        new("1 Yıldan Az",     "1 Yıldan Az"),
        new("1-3 Yıl",         "1-3 Yıl"),
        new("3-5 Yıl",         "3-5 Yıl"),
        new("5-10 Yıl",        "5-10 Yıl"),
        new("10 Yıl ve Üzeri", "10 Yıl ve Üzeri"),
    ];

    public static readonly List<LookupItemViewModel> HouseStatusOptions =
    [
        new("0", "Kendi Evim"),
        new("1", "Kira"),
        new("2", "Aile Evi"),
    ];
}
