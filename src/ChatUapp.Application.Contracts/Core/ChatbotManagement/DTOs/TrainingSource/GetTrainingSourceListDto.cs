using System;
using Volo.Abp.Application.Dtos;

namespace ChatUapp.Core.ChatbotManagement.DTOs.TrainingSource;

public class GetTrainingSourceListDto : PagedAndSortedResultRequestDto
{
    public Guid ChatbotId { get; set; }
    public string? TrainingSourceTitle { get; set; }
}