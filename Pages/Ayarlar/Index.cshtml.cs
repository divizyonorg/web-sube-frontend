<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc.RazorPages;
=======
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;
using MyApp.Web.ViewModels;
using MyApp.Web.ViewModels.Components;
>>>>>>> 0df707417253d89b65e5e402be09791ae848793f

namespace MyApp.Web.Pages.Ayarlar;

public class IndexModel : PageModel
{
<<<<<<< HEAD
    public void OnGet() { }
=======
    private readonly ICustomerDataService _customerDataService;
    private readonly IAuthService _authService;

    public AyarlarViewModel Settings { get; set; } = new();

    public SelectViewModel CalismaDurumu { get; } = new()
    {
        Label = "ÇALIŞMA DURUMU",
        Name = "calismaDurumu",
        AlpineModel = "calisma",
        Options =
        [
            new() { Value = "ucretli",   Label = "Ücretli" },
            new() { Value = "serbest",   Label = "Serbest Meslek / Esnaf" },
            new() { Value = "emekli",    Label = "Emekli" },
            new() { Value = "ev-hanimi", Label = "Ev Hanımı" },
            new() { Value = "ogrenci",   Label = "Öğrenci" },
            new() { Value = "issiz",     Label = "İşsiz" },
        ]
    };

    public SelectViewModel CalismaSektoru { get; } = new()
    {
        Label = "ÇALIŞMA SEKTÖRÜ",
        Name = "calismaSektoru",
        AlpineModel = "sektor",
    };

    public SelectViewModel Meslek { get; } = new()
    {
        Label = "MESLEK",
        Name = "meslek",
        AlpineModel = "meslekId",
    };

    public SelectViewModel CalismaSuresi { get; } = new()
    {
        Label = "ÇALIŞMA SÜRESİ",
        Name = "calismaSuresi",
        AlpineModel = "suresi",
        Options =
        [
            new() { Value = "1y-alti",   Label = "1 yıldan az" },
            new() { Value = "1-3y",      Label = "1–3 yıl" },
            new() { Value = "3-5y",      Label = "3–5 yıl" },
            new() { Value = "5-10y",     Label = "5–10 yıl" },
            new() { Value = "10y-uzeri", Label = "10 yıl ve üzeri" },
        ]
    };

    public SelectViewModel MaasBankasi { get; } = new()
    {
        Label = "MAAŞ BANKASI",
        Name = "maasBankasi",
    };

    public SelectViewModel MedeniHal { get; } = new()
    {
        Label = "MEDENİ HAL",
        Name = "medeniHal",
        AlpineModel = "medeni",
        Options =
        [
            new() { Value = "false", Label = "Bekar" },
            new() { Value = "true",  Label = "Evli" },
        ]
    };

    public RadioViewModel EsCalisiyor { get; } = new()
    {
        Name = "esCalisiyor",
        SelectedValue = "evet",
        Options =
        [
            new() { Value = "evet",  Label = "Evet" },
            new() { Value = "hayir", Label = "Hayır" },
        ]
    };

    public ButtonViewModel KaydetButton { get; } = new()
    {
        Label = "Kaydet",
        Variant = ButtonVariant.Primary,
        Width = 140,
        Height = 44
    };

    public ButtonViewModel ProfilKaydetButton { get; } = new()
    {
        Label = "Kaydet",
        Variant = ButtonVariant.Primary,
        Width = 140,
        Height = 44
    };

    [BindProperty] public string ProfilGsm { get; set; } = string.Empty;
    [BindProperty] public string ProfilEmail { get; set; } = string.Empty;
    [BindProperty] public string GuvenlikGsm { get; set; } = string.Empty;
    [BindProperty] public string GuvenlikOtpCode { get; set; } = string.Empty;
    [BindProperty] public bool FinansalMaritalStatus { get; set; }
    [BindProperty] public bool FinansalIsWorking { get; set; }
    [BindProperty] public decimal FinansalWSalaryAmount { get; set; }
    [BindProperty] public int FinansalWorkSector { get; set; }
    [BindProperty] public int FinansalOccupationId { get; set; }
    [BindProperty] public string FinansalTotalWorkingTime { get; set; } = string.Empty;
    [BindProperty] public int KvkkChannelId { get; set; }
    [BindProperty] public bool KvkkEmail { get; set; }
    [BindProperty] public bool KvkkSms { get; set; }
    [BindProperty] public bool KvkkCall { get; set; }
    [BindProperty] public bool KvkkAdress { get; set; }

    public IndexModel(ICustomerDataService customerDataService, IAuthService authService)
    {
        _customerDataService = customerDataService;
        _authService = authService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        ViewData["Title"] = "Ayarlar";
        ViewData["ActivePage"] = "Ayarlar";

        var workSectorsTask = _customerDataService.GetWorkSectorsAsync();
        var occupationsTask = _customerDataService.GetOccupationsAsync();
        var banksTask = _customerDataService.GetBanksAsync();
        var profileTask = _customerDataService.GetProfileAsync();
        var kvkkTask = _customerDataService.GetKvkkAsync();

        await Task.WhenAll(workSectorsTask, occupationsTask, banksTask, profileTask, kvkkTask);

        CalismaSektoru.Options.AddRange(workSectorsTask.Result);
        Meslek.Options.AddRange(occupationsTask.Result);
        MaasBankasi.Options.AddRange(banksTask.Result);
        Settings.Profil = profileTask.Result;
        Settings.Bildirimler = kvkkTask.Result;

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateContactAsync()
    {
        var success = await _customerDataService.UpdateContactAsync(ProfilGsm, ProfilEmail);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnPostUpdateGsmAsync()
    {
        var success = await _customerDataService.UpdateGsmAsync(GuvenlikGsm);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnPostUpdateMaritalStatusAsync()
    {
        var success = await _customerDataService.UpdateMaritalStatusAsync(
            FinansalMaritalStatus, FinansalIsWorking, FinansalWSalaryAmount);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnPostUpdateWorkAsync()
    {
        var success = await _customerDataService.UpdateWorkAsync(
            FinansalWorkSector, FinansalOccupationId, FinansalTotalWorkingTime);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnPostUpdateKvkkAsync()
    {
        var success = await _customerDataService.UpdateKvkkAsync(KvkkChannelId, KvkkEmail, KvkkSms, KvkkCall, KvkkAdress);
        return new JsonResult(new { success });
    }

    public async Task<IActionResult> OnPostVerifyGsmOtpAsync()
    {
        var (success, _, message) = await _authService.VerifyOtpAsync(GuvenlikGsm, GuvenlikOtpCode);
        return new JsonResult(new { success, message });
    }
>>>>>>> 0df707417253d89b65e5e402be09791ae848793f
}
