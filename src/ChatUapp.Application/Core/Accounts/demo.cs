using System.Collections.Generic;

namespace ChatUapp.Core.Accounts;

public class DemoContext
{
    public List<Group> Groups { get; set; } = new List<Group>();
    public GroupBuilder BuildGroup(string groupName)
    {
        var group = new Group { Name = groupName };
        Groups.Add(group);
        return new GroupBuilder(group, this);
    }

    public List<Group> Finish()
    {
        return Groups;
    }
}

public class GroupBuilder
{
    public Group _group;
    public PermissionDefinition _lastPermission;
    public DemoContext _context;
    public GroupBuilder(Group group, DemoContext context)
    {
        _group = group;
        _context = context;
    }

    public PermissionDefinitionBuilder AddPermission(string name, string dname)
    {
        _lastPermission = new PermissionDefinition(name, dname);
        _group.Permissions.Add(_lastPermission);
        return new PermissionDefinitionBuilder(_lastPermission, this, _context);
    }

    public DemoContext EndGroup()
    {
        return _context;
    }
}

public class PermissionDefinitionBuilder
{
    public PermissionDefinition _permission;
    public PermissionDefinition _upperpermission;
    public GroupBuilder _group;
    public DemoContext _context;

    public PermissionDefinitionBuilder(PermissionDefinition permission, GroupBuilder group, DemoContext context)
    {
        _permission = permission;
        _group = group;
        _context = context;
    }
    public PermissionDefinitionBuilder AddChildPermission(string name, string dname)
    {
        var child = new PermissionDefinition(name, dname);
        child.Children.Add(child);
        return this;
    }

    public GroupBuilder End()
    {
        return _group;
    }
}

public class Group
{
    public string Name { get; set; }
    public List<PermissionDefinition> Permissions { get; set; } = new List<PermissionDefinition>();
}

public class PermissionDefinition
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public List<PermissionDefinition> Children { get; set; } = new List<PermissionDefinition>();
    public PermissionDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }
}

public class Test
{
    public void make()
    {
        var context = new DemoContext()
            .BuildGroup("Test Group")
                .AddPermission("mihad", "ok").End()
                .AddPermission("f", "ff")
                    .AddChildPermission("fff", "dd")
                    .AddChildPermission("ddd", "dd")
                    .AddChildPermission("ddd", "dddd")
                 .End()
             .EndGroup()
             .BuildGroup("one")
                .AddPermission("dd", "dd").End()
             .EndGroup()
             .Finish();

        var context1 = new DemoContext()
            .BuildGroup("mamun")
                .AddPermission("mm", "")
                    .AddChildPermission("m", "m")
                    .AddChildPermission("m1", "m1")
                    .AddChildPermission("m2", "m2")
                .End()
            .EndGroup()
            .Finish();


    }
}
