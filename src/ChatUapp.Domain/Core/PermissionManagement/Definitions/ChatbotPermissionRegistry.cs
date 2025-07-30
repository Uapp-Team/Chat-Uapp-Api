using System.Collections.Generic;
using System.Linq;

namespace ChatUapp.Core.PermissionManagement.Definitions;

public static class ChatbotPermissionRegistry
{
    private static readonly List<PermissionGroupDefinition> _groups = new();
    public static IReadOnlyList<PermissionGroupDefinition> Groups => _groups;

    public static void Initialize(IEnumerable<ChatbotPermissionDefinitionProvider> providers)
    {
        _groups.Clear();

        foreach (var provider in providers)
        {
            var context = new ChatbotPermissionDefinitionContext();
            provider.Define(context);
            _groups.AddRange(context.Groups);
        }
    }

    public static bool IsValid(string permissionName)
    {
        return Groups.SelectMany(g => Flatten(g.Permissions)).Any(p => p.Name == permissionName);
    }

    private static IEnumerable<PermissionDefinition> Flatten(IEnumerable<PermissionDefinition> permissions)
    {
        foreach (var perm in permissions)
        {
            yield return perm;
            foreach (var child in Flatten(perm.Children))
                yield return child;
        }
    }
}
