using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class ChatbotPermissionDefinitionContext
{
    private readonly List<PermissionGroupDefinition> _groups = new();

    public IReadOnlyList<PermissionGroupDefinition> Groups => _groups;

    public PermissionGroupDefinition AddGroup(string name, string displayName, bool isMenu, string menuDisplayName)
    {
        var group = new PermissionGroupDefinition(name, displayName, isMenu, menuDisplayName);
        _groups.Add(group);
        return group;
    }
}
