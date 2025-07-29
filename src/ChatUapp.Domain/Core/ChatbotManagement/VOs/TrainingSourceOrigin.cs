using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.Guards;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace ChatUapp.Core.ChatbotManagement.VOs;

public class TrainingSourceOrigin : ValueObject
{
    public SourceType SourceType { get; }
    public string? SourceUrl { get; }          // For Web
    public string? FileName { get; }           // For File
    public string? FileType { get; }           // For File
    public string? TextContent { get; }        // For Text and Web

    private TrainingSourceOrigin() { } // Required for EF Core

    private TrainingSourceOrigin(
        SourceType sourceType,
        string? sourceUrl = null,
        string? fileName = null,
        string? fileType = null,
        string? textContent = null)
    {
        SourceType = sourceType;
        SourceUrl = sourceUrl;
        FileName = fileName;
        FileType = fileType;
        TextContent = textContent;
    }

    public static TrainingSourceOrigin CreateWebSource(string url, string textContent)
    {
        Ensure.NotNullOrEmpty(url, nameof(url));
        Ensure.NotNullOrEmpty(textContent, nameof(textContent));

        return new TrainingSourceOrigin(SourceType.Web, sourceUrl: url, textContent: textContent);
    }

    public static TrainingSourceOrigin CreateFileSource(string fileName, string fileType)
    {
        Ensure.NotNullOrEmpty(fileName, nameof(fileName));
        Ensure.NotNullOrEmpty(fileType, nameof(fileType));

        return new TrainingSourceOrigin(SourceType.File, fileName: fileName, fileType: fileType);
    }

    public static TrainingSourceOrigin CreateTextSource(string textContent)
    {
        Ensure.NotNullOrEmpty(textContent, nameof(textContent));

        return new TrainingSourceOrigin(SourceType.Text, textContent: textContent);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return SourceType;
        yield return SourceUrl ?? string.Empty;
        yield return FileName ?? string.Empty;
        yield return FileType ?? string.Empty;
        yield return TextContent ?? string.Empty;
    }
}
