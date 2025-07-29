using ChatUapp.Core.PermissionManagement.Definitions;

namespace ChatUapp.Core.PermissionManagement.DefinitionProviders;

public class MyChatbotPermissionDefinitionProvider : ChatbotPermissionDefinitionProvider
{
    public override void Define(ChatbotPermissionDefinitionContext context)
    {
        /*
          context.AddGroup("Group name", "Group display name")
              .WithPermissions(p => p
                  .Add("Permission name", "Permission display name")
                  .Add("Permission name", "Permission display name", child => child
                      .Add("Child permission name","Child permisison display name")
                      .Add("Child permission name", "Child permission display name"))

          );
        */

        context.AddGroup("Chatbot.Dashboard", "Dashboard")
          .WithPermissions(p => p
              .Add("Chatbot.Dashboard.View", "View Dasboard")
              .Add("Chatbot.Analytics.View", "View Analytics")
          );

        context.AddGroup("Chatbot.Chats", "Chats")
          .WithPermissions(p => p
              .Add("Chatbot.Chats.AllChat", "View All Chat")
              .Add("Chatbot.Chats.Details", "View Chat Details")
          );

        context.AddGroup("Chatbot.TrainingCenter", "Training Center")
          .WithPermissions(p => p
              .Add("Chatbot.TrainingCenter.Train", "Add Train Source")
              .Add("Chatbot.TrainingCenter.Edit", "Edit Train Source")
              .Add("Chatbot.TrainingCenter.Delete", "Delete Train Source")
          );
    }
}
