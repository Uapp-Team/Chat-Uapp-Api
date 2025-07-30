using System;
using System.Collections.Generic;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public class PermissionGroupDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public List<PermissionDefinition> Permissions { get; } = new();

    internal PermissionGroupDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public PermissionGroupDefinition AddPermission(string name, string displayName)
    {
        var perm = new PermissionDefinition(name, displayName);
        Permissions.Add(perm);
        return this;
    }

    public PermissionGroupDefinition WithPermissions(Action<PermissionListBuilder> buildAction)
    {
        var builder = new PermissionListBuilder(Permissions);
        buildAction(builder);
        return this;
    }
}

public class PermissionListBuilder
{
    private readonly List<PermissionDefinition> _permissionList;

    public PermissionListBuilder(List<PermissionDefinition> list)
    {
        _permissionList = list;
    }

    public PermissionListBuilder Add(string name, string displayName, Action<PermissionListBuilder>? children = null)
    {
        var permission = new PermissionDefinition(name, displayName);

        if (children != null)
        {
            var childBuilder = new PermissionListBuilder(permission.Children);
            children(childBuilder);
        }

        _permissionList.Add(permission);
        return this;
    }
}