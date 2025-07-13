using ChatUapp.Core.Accounts.DTOs.ApiRequestsDto;
using ChatUapp.Core.Accounts.DTOs.ApiResponsesDto;
using System.Threading.Tasks;

namespace ChatUapp.Core.Accounts.Interfaces;

public interface IOtpAppService
{
    Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto input);
    Task<SendOtpResponseDto> ReSentOtpAsync(SendOtpRequestDto input);
    Task<bool> VerifyOtpAsync(VerifyOtpRequestDto input);
}
