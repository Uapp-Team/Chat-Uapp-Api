using System;

namespace ChatUapp.Core.Accounts.DTOs;

public class CurrentTenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
