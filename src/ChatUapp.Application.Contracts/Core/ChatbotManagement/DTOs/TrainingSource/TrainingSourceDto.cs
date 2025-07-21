using ChatUapp.Core.ChatbotManagement.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.DTOs.TrainingSource;

public class TrainingSourceDto : EntityDto<Guid>
{
    public Guid ChatbotId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public SourceType SourceType { get; set; }
    public string? TextContent { get; set; }
    public DateTime LastUpdated { get; set; }
}