using Jose;

namespace budz_backend.Utils.Consts;

public static class Utils
{
    public const string SESSION_KEY = "user-id";
    public const int MAX_JWT_TTL = 3;
    public const int MAX_USERNAME_LEN = 35;
    public const int MAX_PASSWORD_LEN = 64;
    public const int MIN_PASSWORD_LEN = 7;
    public const string USERNAME_REGEX = "^[A-Za-z0-9_]";

    public const int MAX_NOTIFICATION_TITLE_LEN = 40;
    public const int MIN_NOTIFICATION_TITLE_LEN = 10;

}

