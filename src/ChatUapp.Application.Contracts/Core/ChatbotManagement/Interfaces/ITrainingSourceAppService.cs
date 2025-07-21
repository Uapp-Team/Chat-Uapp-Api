using ChatUapp.Core.ChatbotManagement.DTOs.TrainingSource;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ChatUapp.Core.ChatbotManagement.Interfaces;

public interface ITrainingSourceAppService : IApplicationService
{
    Task<TrainingSourceDto> CreateAsync(CreateTrainingSourceDto input);
    Task<TrainingSourceDto> UpdateAsync(Guid id, UpdateTrainingSourceDto input);
    Task<PagedResultDto<TrainingSourceDto>> GetListAsync(GetTrainingSourceListDto input);
    Task<TrainingSourceDto> GetAsync(Guid id);
}

