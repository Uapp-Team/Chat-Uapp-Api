using System.Text.Json.Serialization;

namespace ChatUapp.Message.ApiResponsesDtos;

public class ReplyMessageResponseDto
{
    public string Answer { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string BotName { get; set; } = string.Empty;
}
