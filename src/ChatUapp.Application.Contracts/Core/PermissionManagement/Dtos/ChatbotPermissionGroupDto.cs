using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Dtos;

public class ChatbotPermissionGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool isGranted { get; set; } = false;
    public bool IsMenu { get; set; } = false;
    public List<ChatbotPermissionDto> Permissions { get; set; } = default!;
}
