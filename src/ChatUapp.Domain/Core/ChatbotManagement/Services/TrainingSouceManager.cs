using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Exceptions;
using ChatUapp.Core.Interfaces.Chatbot;
using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class TrainingSourceManager : DomainService
{
    private readonly IDomainGuidGenerator _guidGenerator;
    private readonly IRepository<TrainingSource, Guid> _sourceRepository;

    public TrainingSourceManager(
        IDomainGuidGenerator guidGenerator,
        IRepository<TrainingSource, Guid> sourceRepository)
    {
        _guidGenerator = guidGenerator;
        _sourceRepository = sourceRepository;
    }

    public TrainingSource CreateWebSource(Guid chatbotId, string name, string url, string content)
    {
        if (CurrentTenant.Id == null)
        {
            throw new AppBusinessException("Tenant ID is not set. Ensure you're in a valid tenant context.");
        }

        var origin = TrainingSourceOrigin.CreateWebSource(url, content);

        var source = new TrainingSource(
            _guidGenerator.Create(),
            chatbotId,
            name,
            origin,
            CurrentTenant.Id);

        return source;
    }

    public void UpdateWebSource(TrainingSource source, string name, string url, string content)
    {
        if (source == null)
        {
            throw new AppBusinessException("Source cannot be null.");
        }

        source.SetName(name);
        source.UpdateOrigin(TrainingSourceOrigin.CreateWebSource(url, content));
    }

    public TrainingSource CreateTextSource(Guid chatbotId, string name, string content)
    {
        if (CurrentTenant.Id == null)
        {
            throw new AppBusinessException("Tenant ID is not set. Ensure you're in a valid tenant context.");
        }

        var origin = TrainingSourceOrigin.CreateTextSource(content);

        var source = new TrainingSource(
            _guidGenerator.Create(),
            chatbotId,
            name,
            origin,
            CurrentTenant.Id);

        return source;
    }

    public void UpdateTextSource(TrainingSource source, string name, string content)
    {
        if (source == null)
        {
            throw new AppBusinessException("Source cannot be null.");
        }

        source.SetName(name);
        source.UpdateOrigin(TrainingSourceOrigin.CreateTextSource(content));
    }
}