using System;
using System.Collections.Generic;

namespace ChatUapp.Core.Accounts.DTOs;

public class CurrentUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IReadOnlyList<string> Roles { get; set; } = default!;
}
