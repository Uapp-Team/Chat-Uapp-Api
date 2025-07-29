using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class PermissionDefinition
{
    internal string Name { get; }
    internal string DisplayName { get; }
    internal List<PermissionDefinition> Children { get; } = new();

    internal PermissionDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    internal PermissionDefinition AddChild(string name, string displayName)
    {
        var child = new PermissionDefinition(name, displayName);
        Children.Add(child);
        return child;
    }
}

