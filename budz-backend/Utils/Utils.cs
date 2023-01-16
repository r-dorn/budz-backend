using System.Text.Json.Serialization;
using Jose;
using MongoDB.Bson.IO;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace budz_backend.Utils.Consts;



public static class Utils
{
    public const string SESSION_KEY = "user-id";
    public const int MAX_JWT_TTL = 3;
    public const int MAX_USERNAME_LENGTH = 32;
    public const string USERNAME_REGEX = @"^[-a-zA-Z0-9]";



    public static string GetToken(string subject, byte[] secretKey)
    {

        var currentTime = DateTimeOffset.UtcNow.AddHours(MAX_JWT_TTL).ToUnixTimeSeconds();
        var payload = new Dictionary<string, object>()
        {
            { "exp", currentTime},
            {"sub", subject}
        };
        return JWT.Encode(payload, secretKey, JwsAlgorithm.RS256);
    }


}

