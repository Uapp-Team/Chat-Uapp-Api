﻿namespace ChatUapp.Accounts.DTOs.ApiResponsesDto;

public class SendOtpResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
