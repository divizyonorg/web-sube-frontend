namespace MyApp.Web.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Message)> SendOtpAsync(string phoneNumber, string tckn);
    Task<(bool Success, string? Token, string? Message)> VerifyOtpAsync(string phoneNumber, string otpCode);
}
