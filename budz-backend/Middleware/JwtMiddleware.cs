using budz_backend.Services.MongoServices.User;
using Jose;
using Microsoft.Extensions.Options;
using Settings = budz_backend.Models.Settings;

namespace budz_backend.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string Key;

    public JwtMiddleware(RequestDelegate next, IOptions<Settings.Jwt.JwtSettings> settings)
    {
        _next = next;
        Key = settings.Value.Key;
    }


    public async Task InvokeAsync(HttpContext context, UserService serv)
    {
        try
        {
            var rawHeader = context.Request.Headers.Authorization.ToString();

            var token = rawHeader.Split(" ").Last();

            var deserializedBody = JWT.Decode<Dictionary<string, object>>(token);
            var userID = deserializedBody["sub"].ToString();
            if (userID is null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("invalid user id provided");
                return;
            }

            var user = await serv.GetAsync(userID);
            if (user.IsBanned)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("user is banned");
                return;
            }

            context.Items[Utils.Consts.Utils.SESSION_KEY] = user.Id;
            await _next(context);
        }
        catch (ArgumentException)
        {
            await _next(context);
        }
    }
}