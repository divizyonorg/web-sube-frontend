using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Models.Register;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Register;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly ICustomerRegistrationService _registrationService;
    private readonly IAuthService _authService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] public string? FirstName { get; set; }
    [BindProperty] public string? LastName { get; set; }
    [BindProperty] public string? Email { get; set; }
    [BindProperty] public string? BirthDate { get; set; }
    [BindProperty] public string? Tckn { get; set; }
    [BindProperty] public string? PhoneNumber { get; set; }
    [BindProperty] public string? OtpCode { get; set; }
    [BindProperty] public bool ConsentOpenRiza { get; set; }
    [BindProperty] public bool ConsentAydinlatma { get; set; }
    [BindProperty] public bool ConsentIleti { get; set; }
    [BindProperty] public bool ConsentSms { get; set; }
    [BindProperty] public bool ConsentEposta { get; set; }
    [BindProperty] public bool ConsentArama { get; set; }

    public IndexModel(ICustomerRegistrationService registrationService, IAuthService authService, ILogger<IndexModel> logger)
    {
        _registrationService = registrationService;
        _authService = authService;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(Request.Cookies["auth_token"]))
            return Redirect("/anasayfa");
        return Page();
    }

    public async Task<IActionResult> OnPostSendOtpAsync()
    {
        _logger.LogInformation("Register SendOtp: Phone='{Phone}'", PhoneNumber);
        var (success, message, isCustomer) = await _authService.SendOtpAsync(PhoneNumber!);
        return new JsonResult(new { success, message, isCustomer });
    }

    public async Task<IActionResult> OnPostVerifyOtpAsync()
    {
        _logger.LogInformation("Register VerifyOtp: Phone='{Phone}'", PhoneNumber);
        var (success, token, message) = await _authService.VerifyOtpAsync(PhoneNumber!, OtpCode!);

        if (!success)
            return new JsonResult(new { success = false, message });

        return new JsonResult(new { success = true, token });
    }

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        _logger.LogInformation("Register → tckn='{Tckn}' phone='{Phone}'", Tckn, PhoneNumber);

        var createRequest = new CreateCustomerRequest
        {
            Tckn = Tckn ?? string.Empty,
            FirstName = FirstName ?? string.Empty,
            LastName = LastName ?? string.Empty,
            Birthday = FormatBirthday(BirthDate),
            Gsm = FormatGsm(PhoneNumber),
            App = "web",
            MapVisit = "true"
        };

        var (createSuccess, createMessage, newToken) = await _registrationService.CreateCustomerAsync(createRequest);
        if (!createSuccess)
            return new JsonResult(new { success = false, message = createMessage ?? "Kayıt oluşturulamadı." });

        var kvkkRequest = new KvkkRequest
        {
            ChannelId = 3,
            Call = ConsentArama,
            Email = ConsentEposta,
            Adress = ConsentIleti,
            Sms = ConsentSms
        };

        var (kvkkSuccess, kvkkMessage) = await _registrationService.UpdateKvkkAsync(kvkkRequest, newToken);
        if (!kvkkSuccess)
            _logger.LogWarning("KVKK güncelleme başarısız: {Message}", kvkkMessage);

        var contactRequest = new ContactRequest { Email = Email ?? string.Empty };
        var (contactSuccess, contactMessage) = await _registrationService.UpdateContactAsync(contactRequest, newToken);
        if (!contactSuccess)
            _logger.LogWarning("Contact güncelleme başarısız: {Message}", contactMessage);

        return new JsonResult(new { success = true, token = newToken });
    }

    // "23/05/2003" → "2003-05-23"
    private static string FormatBirthday(string? birthDate)
    {
        if (string.IsNullOrWhiteSpace(birthDate)) return string.Empty;
        return DateTime.TryParseExact(birthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
            ? dt.ToString("yyyy-MM-dd")
            : birthDate;
    }

    // "+90 537 040 51 97" → "05370405197"
    private static string FormatGsm(string? phoneNumber)
    {
        var digits = Regex.Replace(phoneNumber ?? string.Empty, @"\D", "");
        return digits.Length >= 12 ? "0" + digits[2..] : digits;
    }
}
