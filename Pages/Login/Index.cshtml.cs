using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Web.Services.Interfaces;

namespace MyApp.Web.Pages.Login;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ICustomerCheckService _customerCheckService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] public string? PhoneNumber { get; set; }
    [BindProperty] public string? Tckn { get; set; }
    [BindProperty] public string? OtpCode { get; set; }
    [BindProperty] public bool RememberMe { get; set; }

    public IndexModel(IAuthService authService, ICustomerCheckService customerCheckService, ILogger<IndexModel> logger)
    {
        _authService = authService;
        _customerCheckService = customerCheckService;
        _logger = logger;
    }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostSendOtpAsync()
    {
        _logger.LogInformation("SendOtp handler: Phone='{Phone}'", PhoneNumber);
        var (success, message, isCustomer) = await _authService.SendOtpAsync(PhoneNumber!);
        return new JsonResult(new { success, message, isCustomer });
    }

    public async Task<IActionResult> OnPostVerifyOtpAsync()
    {
        _logger.LogInformation("VerifyOtp handler: Phone='{Phone}'", PhoneNumber);

        var (success, token, message) = await _authService.VerifyOtpAsync(PhoneNumber!, OtpCode!);

        if (!success)
            return new JsonResult(new { success = false, message });

        var tcknMatch = await _customerCheckService.CheckTcknMatchAsync(Tckn!, token);

        if (!tcknMatch)
            return new JsonResult(new { success = false, tcknMismatch = true, message = "TC bilgisi eşleşmemektedir." });

        return new JsonResult(new { success = true, token });
    }

    public IActionResult OnPost() => Page();
}
