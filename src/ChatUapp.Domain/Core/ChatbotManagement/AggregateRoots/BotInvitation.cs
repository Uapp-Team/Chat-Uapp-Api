using ChatUapp.Core.Guards;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.ChatbotManagement.AggregateRoots;

public class BotInvitation : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid BotId { get; private set; }
    public string UserEmail { get; private set; } = default!;
    public string Role { get; private set; } = default!;
    public bool IsRegistered { get; private set; } = false;
    public string InvitationToken { get; private set; } = default!;
    public Guid? InvitedBy { get; private set; }
    public Guid? TenantId { get; private set; }

    private BotInvitation() { }

    internal BotInvitation(Guid id, Guid? InvitedBy, Guid ? TenantId, Guid botId, string userEmail, string role, bool isRegistered = false)
        : base(id)
    {
        SetBotId(botId);
        SetInvitedBy(InvitedBy);
        SetTenantId(InvitedBy);
        SetEmail(userEmail);
        SetRole(role);
        IsRegistered = isRegistered;
        RegenerateToken();
    }

    private void SetBotId(Guid botId)
    {
        Ensure.NotNull(botId, nameof(botId));
        BotId = botId;
    }

    private void SetInvitedBy(Guid? userId)
    {
        Ensure.NotNull(userId, nameof(userId));
        InvitedBy = userId;
    }

    private void SetTenantId(Guid? tenantId)
    {
        Ensure.NotNull(tenantId, nameof(tenantId));
        TenantId = tenantId;
    }

    private void SetEmail(string email)
    {
        Ensure.NotNullOrEmpty(email, nameof(email));
        UserEmail = email.Trim().ToLowerInvariant();
    }

    private void SetRole(string role)
    {
        Ensure.NotNullOrEmpty(role, nameof(role));
        Role = role;
    }

    internal void MarkAsRegistered()
    {
        IsRegistered = true;
    }

    internal void Unregister()
    {
        IsRegistered = false;
    }

    private void RegenerateToken()
    {
        InvitationToken = Guid.NewGuid().ToString("N");
    }

    internal void UpdateInvitation(string email, string role)
    {
            SetEmail(email);
            SetRole(role);
        RegenerateToken();
    }
}
