using ChatUapp.Core.Constants;

namespace ChatUapp.Core.ChatbotManagement.Consts;

public class TrainingSourceConsts
{
    public const int NameMaxLength = AppConsts.NameMaxLength;
    public const int DescriptionMaxLength = AppConsts.DesriptionMaxLength;
    public const string DefaultName = "Default Training Source";
    public const string DefaultDescription = "This is a default training source for the chatbot.";

    // 🧠 Value Object: TrainingSourceOrigin
    public const int SourceUrlMaxLength = 500; // For Web sources
    public const int FileNameMaxLength = 200; // For File sources
    public const int FileTypeMaxLength = 100; // For File sources

    public const string OriginTextContentColumnType = AppConsts.TextColumnType; // For large text content in PostgreSQL

    public const string OriginSourceTypeColumnName = "Origin_SourceType";
    public const string OriginSourceUrlColumnName = "Origin_SourceUrl";
    public const string OriginFileNameColumnName = "Origin_FileName";
    public const string OriginFileTypeColumnName = "Origin_FileType";
    public const string OriginTextContentColumnName = "Origin_TextContent";



}
