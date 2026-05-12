namespace MyApp.Web.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Message, bool IsCustomer)> SendOtpAsync(string phoneNumber);
    Task<(bool Success, string? Token, string? Message)> VerifyOtpAsync(string phoneNumber, string otpCode);
}
