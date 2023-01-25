using Jose;

namespace budz_backend.Utils.Consts;

public static class Utils
{
    public const string SESSION_KEY = "user-id";
    public const int MAX_JWT_TTL = 3;


    public static string GetToken(string subject, byte[] secretKey)
    {
        var currentTime = DateTimeOffset.UtcNow.AddHours(MAX_JWT_TTL).ToUnixTimeSeconds();
        var payload = new Dictionary<string, object>
        {
            { "exp", currentTime },
            { "sub", subject }
        };
        return JWT.Encode(payload, secretKey, JwsAlgorithm.RS256);
    }


    public static Dictionary<string, object>? DecodeToken<KeyType>(HttpContext ctx, KeyType key)
    {
        var rawHeader = ctx.Request.Headers.Authorization.ToString();
        if (rawHeader == string.Empty) return null;
        return JWT.Decode<Dictionary<string, object>>(rawHeader.Split(" ").Last());
    }
}

