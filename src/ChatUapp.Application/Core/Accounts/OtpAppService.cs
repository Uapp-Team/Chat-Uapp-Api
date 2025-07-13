using ChatUapp.Core.Accounts.DTOs.ApiRequestsDto;
using ChatUapp.Core.Accounts.DTOs.ApiResponsesDto;
using ChatUapp.Core.Accounts.Interfaces;
using ChatUapp.Core.Emailing.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;

namespace ChatUapp.Core.Accounts;

public class OtpAppService : ApplicationService, IOtpAppService
{
    private readonly IAppEmailSender _emailSender;
    private readonly IDistributedCache<string> _otpCache;
    private readonly IdentityUserManager _identityUser;
    public OtpAppService(
        IAppEmailSender emailSender,
        IDistributedCache<string> otpCache,
        IdentityUserManager identityUser)
    {
        _emailSender = emailSender;
        _otpCache = otpCache;
        _identityUser = identityUser;
    }

    public async Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto input)
    {
        var throttleKey = GetThrottleKey(input.Email);

        if (await _otpCache.GetAsync(throttleKey) != null)
        {
            return new SendOtpResponseDto
            {
                Success = false,
                Message = "You must wait a few minutes before requesting another OTP."
            };
        }

        return await GenerateAndSendOtpAsync(input.Email, throttleKey);
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequestDto input)
    {
        var otpKey = GetOtpKey(input.Email);
        var cachedOtp = await _otpCache.GetAsync(otpKey);

        if (cachedOtp == null)
            return false;

        if (cachedOtp == input.Otp)
        {
            await _otpCache.RemoveAsync(otpKey); // One-time use
            return true;
        }

        return false;
    }

    public async Task<SendOtpResponseDto> ReSentOtpAsync(SendOtpRequestDto input)
    {
        var throttleKey = GetThrottleKey(input.Email);

        // Clear throttle to allow immediate resend
        await _otpCache.RemoveAsync(throttleKey);

        return await GenerateAndSendOtpAsync(input.Email, throttleKey);
    }

    private static string GetThrottleKey(string email) => $"OTP_THROTTLE_{email}";
    private static string GetOtpKey(string email) => $"OTP_{email}";
    private static string GenerateOtp()
    {
        return new Random().Next(100000, 999999).ToString();
    }
    private async Task<SendOtpResponseDto> GenerateAndSendOtpAsync(string email, string throttleKey)
    {
        var user = await _identityUser.FindByEmailAsync(email);
        if (user != null)
        {
            return new SendOtpResponseDto
            {
                Success = true,
                Message = $"The email address '{email}' is already registered."
            };
        }

        var otp = GenerateOtp();
        var otpKey = GetOtpKey(email);

        await _otpCache.SetAsync(otpKey, otp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        await _otpCache.SetAsync(throttleKey, "true", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });

        await _emailSender.SendAsync(
            to: email,
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
}
