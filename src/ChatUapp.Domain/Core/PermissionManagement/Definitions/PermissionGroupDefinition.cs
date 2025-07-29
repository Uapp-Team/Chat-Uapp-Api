using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class PermissionGroupDefinition
{
    internal string Name { get; }
    internal string DisplayName { get; }
    internal List<PermissionDefinition> Permissions { get; } = new();

    internal PermissionGroupDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    internal PermissionDefinition AddPermission(string name, string displayName)
    {
        var perm = new PermissionDefinition(name, displayName);
        Permissions.Add(perm);
        return perm;
    }
}
