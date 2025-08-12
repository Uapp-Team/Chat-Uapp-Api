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

        context.AddGroup("Chatbot.Overview", "Overview", true, "Overview")
          .WithPermissions(p => p
              .Add("Chatbot.Dashboard.View", "View Dasboard", true, "Dashboard")
              .Add("Chatbot.Analytics.View", "View Analytics", true, "Analytics")
          );

        context.AddGroup("Chatbot.Chats", "Chats", true, "Chats")
          .WithPermissions(p => p
              .Add("Chatbot.Chats.AllChat", "View All Chat", true, "All Chats")
              .Add("Chatbot.Chats.Details", "View Chat Details", false, "Chat Details")
          );

        context.AddGroup("Chatbot.TrainingCenter", "Training Center", true, "Training Center")
          .WithPermissions(p => p
              .Add("Chatbot.TrainingCenter.View", "View Trained Sources", false, "Training Center")
              .Add("Chatbot.TrainingCenter.Train", "Add Train Source", false, "Training Center")
              .Add("Chatbot.TrainingCenter.Edit", "Edit Train Source", false, "Training Center")
              .Add("Chatbot.TrainingCenter.Delete", "Delete Train Source", false, "Training Center")
          );

        context.AddGroup("Chatbot.Feedback", "Feedback", true, "Feedback")
          .WithPermissions(p => p
              .Add("Chatbot.Feedback.View", "View Feedback", false, "Feedback")
              .Add("Chatbot.Feedback.Respond", "Respond to Feedback", false, "Feedback")
          );

        context.AddGroup("Chatbot.BotSettings", "Bot Settings", true, "Bot Settings")
          .WithPermissions(p => p
              .Add("Chatbot.BotSettings.ManageUsers.List", "View Chatbot Users", false, "Bot Settings")
              .Add("Chatbot.BotSettings.ManageUsers.ViewPermission", "View User Permission", false, "Bot Settings")
              .Add("Chatbot.BotSettings.ManageUsers.EditPermission", "Edit User Permission", false, "Bot Settings")
          );
    }
}
