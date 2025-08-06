using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class PermissionDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public bool IsMenu { get; }
    public string MenuDisplayName { get; }
    public bool IsGranted { get; set; } = false;
    public List<PermissionDefinition> Children { get; } = new();

    internal PermissionDefinition(string name, string displayName, bool isMenu, string menuDisplayName)
    {
        Name = name;
        DisplayName = displayName;
        IsMenu = isMenu;
        MenuDisplayName = menuDisplayName;
    }

    public PermissionDefinition AddChild(string name, string displayName,bool isMenu, string menuDisplayName)
    {
        var child = new PermissionDefinition(name, displayName,isMenu, menuDisplayName);
        Children.Add(child);
        return child;
    }
}
