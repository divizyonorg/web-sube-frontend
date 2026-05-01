using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Login;

public class IndexModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] public string? PhoneNumber { get; set; }
    [BindProperty] public string? Tckn { get; set; }
    [BindProperty] public string? OtpCode { get; set; }
    [BindProperty] public bool RememberMe { get; set; }

    public IndexModel(IAuthService authService, ILogger<IndexModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostSendOtpAsync()
    {
        _logger.LogInformation("SendOtp handler: Phone='{Phone}' Tckn='{Tckn}'", PhoneNumber, Tckn);
        var (success, message) = await _authService.SendOtpAsync(PhoneNumber!, Tckn!);
        return new JsonResult(new { success, message });
    }

    public async Task<IActionResult> OnPostVerifyOtpAsync()
    {
        _logger.LogInformation("VerifyOtp handler: Phone='{Phone}' OtpCode='{OtpCode}'", PhoneNumber, OtpCode);
        var (success, message) = await _authService.VerifyOtpAsync(PhoneNumber!, OtpCode!);
        return new JsonResult(new { success, message });
    }

    public IActionResult OnPost() => Page();
}
