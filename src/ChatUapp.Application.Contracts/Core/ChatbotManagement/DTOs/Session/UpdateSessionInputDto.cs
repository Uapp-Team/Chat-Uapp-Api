using System;

namespace ChatUapp.Core.ChatbotManagement.DTOs.Session;

public class UpdateSessionInputDto
{
    public Guid sessionId { get; set; }
    public string message { get; set; } = default!;
}

public class LikeDislikeInputDto
{
    public Guid sessionId { get; set; }
    public Guid messageId { get; set; }
}
