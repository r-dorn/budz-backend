using budz_backend.Models.User.Settings;

namespace budz_backend.Models.Notification;

public static class NotificationMessageTemplate
{
    public static Dictionary<NotificationType, string> NOTIFICATION_TITLE = new()
    {
        { NotificationType.StrainRestock, "{0} restocked {1}" },
        { NotificationType.StrainOrder, "{0} placed order" },
        { NotificationType.StrainCreation, "{0} published {1}" },
        { NotificationType.UniqueStrainRequest, "{0} added new strain request" },
        { NotificationType.Comment, "{0} commented on {1}" }
    };
}

public struct UserInformation
{
    public UserInformation(string sentBy, string target)
    {
        SentBy = sentBy;
        TargetID = target;
    }

    public string SentBy { get; set; } = string.Empty;
    public string TargetID { get; set; } = string.Empty;
}

public record InternalNotification
{
    public string NotficationID { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string NotificationBody { get; set; } = string.Empty;
    public UserInformation Information { get; set; }
    public NotificationType Type { get; set; }
    public bool IsSystem { get; set; } = true;

    public string TruncateMessageBody(string body, int newLength, char trailingCharacter, int charRepeat)
    {
        var truncatedString = body.Substring(0, newLength);
        return truncatedString.Concat(new string(trailingCharacter, charRepeat)).ToString()!;
    }
}

public record Notification
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string SentBy { get; set; } = string.Empty;
}

