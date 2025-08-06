using System;
using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Dtos;

public class ChatbotPermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsGranted { get; set; } = false;
    public bool IsMenu { get; set; } = false;
    public List<ChatbotPermissionDto> Children { get; } = default!;
}

public class ChatbotPermissionCreateDto
{
    public Guid UserId { get; set; }
    public Guid ChatbotId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
}
