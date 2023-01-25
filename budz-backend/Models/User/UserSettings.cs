namespace budz_backend.Models.User.Settings;

public enum NotificationType
{
    StrainCreation,
    StrainRestock,
    Comment,
    Reply,
    Like,
    Dislike,
    UniqueStrainRequest,
    StrainOrder
}

public record UserSettings
{
    public bool AllowNotification { get; set; } = true;
    public NotificationType[] AllowedTypes { get; set; } = { };
}

public record AllowNotification
{
    public NotficationType[] Types { get; set; }
}

