namespace ChatUapp.Core.Accounts.DTOs.ApiRequestsDto;

public class VerifyOtpRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
