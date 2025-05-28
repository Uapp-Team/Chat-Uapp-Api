using ChatUapp.Accounts.DTOs.ApiRequestsDto;
using ChatUapp.Accounts.DTOs.ApiResponsesDto;
using System.Threading.Tasks;

namespace ChatUapp.Accounts.Interfaces;

public interface IOtpAppService
{
    Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto input);
    Task<bool> VerifyOtpAsync(VerifyOtpRequestDto input);
}
