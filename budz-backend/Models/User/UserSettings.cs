namespace budz_backend.Models.User.Settings;

public enum NotficationType
{
    StrainCreation,
    StrainRestock,
    Comment,
    Reply,
    Like,
    Dislike,
    UniqueStrainRequest,
    StrainOrder
};

public record UserSettings
{
    public string NotificationChannelID { get; set; } = Guid.NewGuid().ToString();
    public bool AllowNotification { get; set; } = true;
}


