using System.Text.Json.Serialization;
using Jose;

namespace budz_backend.Utils.Consts;



public static class Utils
{
    public const string SESSION_KEY = "user-id";
    public const int MAX_JWT_TTL = 3;
    public const int MAX_USERNAME_LENGTH = 32;
    public const int MIN_USERNAME_LENGTH = 6;

    public const int MIN_PASSWORD_LENGTH = 6;
    public const int MAX_PASSWORD_LENGTH = 64;

    public const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$";
    public const string USERNAME_REGEX = @"^[a-zA-Z0-9]";
    public const int MIN_NOTIFICATION_TITLE_LEN = 5;
    public const int MAX_NOTIFICATION_TITLE_LEN = 20;
    


}

