using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.DTOs.TrainingSource;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.ChatbotManagement.Services;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Extensions;
using ChatUapp.Core.Guards;
using ChatUapp.Core.Thirdparty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Core.ChatbotManagement;

public class TrainingSourceAppService : ApplicationService, ITrainingSourceAppService
{
    private readonly TrainingSourceManager _manager;
    private readonly IRepository<TrainingSource, Guid> _repository;
    private readonly IBotEngineManageService _botEngineManageService;

    public TrainingSourceAppService(
        TrainingSourceManager manager,
        IRepository<TrainingSource, Guid> repository,
        IBotEngineManageService botEngineManageService)
    {
        _manager = manager;
        _repository = repository;
        _botEngineManageService = botEngineManageService;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        Ensure.NotNull(entity, nameof(entity));
        var result = await _botEngineManageService.DeleteTrainAsync(
            entity.Id.ToTrainSourceTitle(), entity.ChatbotId.ToBotName());

        if (!result.Success)
        {
            throw new AppBusinessException("Training source deletion failed: " + result.Message);
        }
        await _repository.DeleteAsync(entity, autoSave: true);
    }

    public async Task<TrainingSourceDto> CreateAsync(CreateTrainingSourceDto input)
    {
        var entity = _manager.CreateTextSource(input.ChatbotId, input.Name, input.TextContent);

        var result = await _botEngineManageService.TrainAsync(new Message.Interfaces.BotTrainRequestModel()
        {
            Text = input.TextContent,
            Title = input.Name,
            BotName = input.ChatbotId.ToBotName()
        });

        if (!result.Success)
        {
            throw new AppBusinessException("Training source creation failed: " + result.Message);
        }

        await _repository.InsertAsync(entity, autoSave: true);
        return MapToDto(entity);
    }

    public async Task<TrainingSourceDto> UpdateAsync(Guid id, UpdateTrainingSourceDto input)
    {
        var entity = await _repository.GetAsync(id);
        Ensure.NotNull(entity, nameof(entity));
        _manager.UpdateTextSource(entity, input.Name, input.TextContent);

        var result = await _botEngineManageService.UpdateTrainAsync(new Message.Interfaces.BotTrainRequestModel()
        {
            Text = input.TextContent,
            Title = input.Name,
            BotName = entity.ChatbotId.ToBotName()
        });

        if (!result.Success)
        {
            throw new AppBusinessException("Training source update failed: " + result.Message);
        }

        await _repository.UpdateAsync(entity, autoSave: true);
        return MapToDto(entity);
    }

    public async Task<PagedResultDto<TrainingSourceDto>> GetListAsync(GetTrainingSourceListDto input)
    {
        var queryable = await _repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.TrainingSourceTitle))
        {
            queryable = queryable.Where(x => x.Name.Contains(input.TrainingSourceTitle));
        }

        var filtered = queryable
            .WhereIf(input.ChatbotId != Guid.Empty, x => x.ChatbotId == input.ChatbotId)
            .OrderByDescending(x => x.LastUpdated)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var items = await AsyncExecuter.ToListAsync(filtered);
        var totalCount = await _repository.CountAsync(x => x.ChatbotId == input.ChatbotId);

        return new PagedResultDto<TrainingSourceDto>(
            totalCount,
            items.Select(MapToDto).ToList()
        );
    }

    public async Task<TrainingSourceDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        Ensure.NotNull(entity, nameof(entity));

        return MapToDto(entity);
    }

    private static TrainingSourceDto MapToDto(TrainingSource entity)
    {
        return new TrainingSourceDto
        {
            Id = entity.Id,
            ChatbotId = entity.ChatbotId,
            Name = entity.Name,
            Description = entity.Description,
            SourceType = entity.Origin.SourceType,
            TextContent = entity.Origin.TextContent,
            LastUpdated = entity.LastUpdated
        };
    }
}

