using ChatUapp.Core.PermisionManagement.Consts;
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
              .Add(ChatbotPermissionConsts.ChatbotDashboardView, "View Dasboard", true, "Dashboard")
              .Add(ChatbotPermissionConsts.ChatbotAnalyticsView,  "View Analytics", true, "Analytics")
          );

        context.AddGroup("Chatbot.Chats", "Chats", true, "Chats")
          .WithPermissions(p => p
              .Add(ChatbotPermissionConsts.ChatbotChatsAllChat, "View Chat Details", false, "Chat Details")
              .Add(ChatbotPermissionConsts.ChatbotChatsDetails, "View Chat Details", false, "Chat Details")
          );

        context.AddGroup("Chatbot.TrainingCenter", "Training Center", true, "Training Center")
          .WithPermissions(p => p
              .Add(ChatbotPermissionConsts.ChatbotTrainingCenterView, "Delete Train Source", false, "Training Center")
              .Add(ChatbotPermissionConsts.ChatbotTrainingCenterTrain, "Delete Train Source", false, "Training Center")
              .Add(ChatbotPermissionConsts.ChatbotTrainingCenterEdit, "Delete Train Source", false, "Training Center")
              .Add(ChatbotPermissionConsts.ChatbotTrainingCenterDelete, "Delete Train Source", false, "Training Center")
          );

        context.AddGroup("Chatbot.Feedback", "Feedback", true, "Feedback")
          .WithPermissions(p => p
              .Add(ChatbotPermissionConsts.ChatbotFeedbackView, "Respond to Feedback", false, "Feedback")
              .Add(ChatbotPermissionConsts.ChatbotFeedbackRespond, "Respond to Feedback", false, "Feedback")
          );

        context.AddGroup("Chatbot.BotSettings", "Bot Settings", true, "Bot Settings")
          .WithPermissions(p => p
              .Add(ChatbotPermissionConsts.ChatbotBotSettingsManageUsersList, "Edit User Permission", false, "Bot Settings")
              .Add(ChatbotPermissionConsts.ChatbotBotSettingsManageUsersViewPermission, "Edit User Permission", false, "Bot Settings")
              .Add(ChatbotPermissionConsts.ChatbotBotSettingsManageUsersEditPermission, "Edit User Permission", false, "Bot Settings")
              .Add(ChatbotPermissionConsts.ChatbotBotSettingsManageUsersInviteUser, "Invitation User Permission", false, "Bot Settings")
          );
    }
}
