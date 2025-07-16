using ChatUapp.Core.Constants;

namespace ChatUapp.Core.ChatbotManagement.Consts;

public class ChatbotConsts
{
    public static int NameMaxLength { get; set; } = AppConsts.NameMaxLength;
    public static int DescriptionMaxLength { get; set; } = AppConsts.DesriptionMaxLength;
    public static int HeaderMaxLength { get; set; } = 300;
    public static int SubHeaderMaxLength { get; set; } = 300;
    public static int UniqueKeyMaxLength { get; set; } = AppConsts.KeyMaxLength;
    public static int BrandImageNameMaxLength { get; set; } = AppConsts.ImageNameMaxLength;

    //Icon styles
    public static int IconNameMaxLength { get; set; } = AppConsts.ImageNameMaxLength;
    public static int IconColorMaxLength { get; set; } = 20;
}
