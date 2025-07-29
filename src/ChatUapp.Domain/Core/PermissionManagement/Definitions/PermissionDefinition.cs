using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class PermissionDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public bool IsGranted { get; set; } = false;
    public List<PermissionDefinition> Children { get; } = new();

    internal PermissionDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public PermissionDefinition AddChild(string name, string displayName)
    {
        var child = new PermissionDefinition(name, displayName);
        Children.Add(child);
        return child;
    }
}

