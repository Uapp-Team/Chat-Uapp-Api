using System;
using System.Threading.Tasks;
using ChatUapp.Accounts.DTOs.ApiRequestsDto;
using ChatUapp.Accounts.DTOs.ApiResponsesDto;
using ChatUapp.Accounts.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;

namespace ChatUapp.Accounts;

public class OtpAppService : ApplicationService, IOtpAppService
{
    private readonly IEmailSender _emailSender;
    private readonly IDistributedCache<string> _otpCache;
    private readonly IdentityUserManager _identityUser;
    public OtpAppService(IEmailSender emailSender, IDistributedCache<string> otpCache, IdentityUserManager identityUser)
    {
        _emailSender = emailSender;
        _otpCache = otpCache;
        _identityUser = identityUser;
    }

    public async Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto input)
    {
        var throttleKey = $"OTP_THROTTLE_{input.Email}";
        var alreadyRequested = await _otpCache.GetAsync(throttleKey);
        if (alreadyRequested != null)
        {
            return new SendOtpResponseDto
            {
                Success = false,
                Message = "You must wait a few minutes before requesting another OTP."
            };
        }

        var user = await _identityUser.FindByEmailAsync(input.Email);
        if(user != null)
        {
            return await Task.FromResult(new SendOtpResponseDto
            {
                Success = true,
                Message = $"The email address '{input.Email}' is already registered.",
            });
        }

        var otp = new Random().Next(100000, 999999).ToString();

        
        
        await _otpCache.SetAsync($"OTP_{input.Email}", otp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        // Throttle new requests for 2 minutes
        await _otpCache.SetAsync(throttleKey, "true", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });

        await _emailSender.SendAsync(
            to: input.Email,
            subject: "Your OTP Code",
            body: $"Your OTP is: {otp}",
            isBodyHtml: false
        );
        return await Task.FromResult(new SendOtpResponseDto
        {
            Success = true,
            Message = "OTP sent successfully",
            Otp = otp
        });
    }


    public async Task<bool> VerifyOtpAsync(VerifyOtpRequestDto input)
    {
        var cachedOtp = await _otpCache.GetAsync($"OTP_{input.Email}");
        if (cachedOtp == null)
        {
            return false;
        }

        if (cachedOtp == input.Otp)
        {
            await _otpCache.RemoveAsync($"OTP_{input.Email}"); // One-time use
            return true;
        }

        return false;
    }
}
