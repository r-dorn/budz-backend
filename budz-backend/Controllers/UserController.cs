


using System.Text;
using budz_backend.Models.User;
using budz_backend.Models.User.Settings;
using budz_backend.Services.MongoServices.User;
using budz_backend.Utils.Consts;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JwtSettings = budz_backend.Models.Settings.Jwt;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService Serv;
    private readonly JwtSettings.JwtSettings Settings;

    public UserController(UserService serv, IOptions<JwtSettings.JwtSettings> settings)
    {
        Serv = serv;
        Settings = settings.Value;
    }


    [HttpPost("signup")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        if (await Serv.DocumentExists("Username", user.Username))
        {
            return Conflict("username already exists");
        }

        MongoUser createdUser = new MongoUser
        {
            Username = user.Username,
            Password = user.Password,
            Role = user.Role
        };
        createdUser.Setup();
        await Serv.InsertAsync(createdUser);
        return Ok("user created");
    }

    [HttpPost("login")]
    public async Task<ActionResult> loginUser([FromBody] LoginRequest user)
    {
        if (!await Serv.DocumentExists("Username", user.Username))
        {
            return Unauthorized("no account found with provided username");
        }

        MongoUser foundUser = await Serv.GetAsync("Username", user.Username);
        if (!BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password))
        {
            return Unauthorized("password or user does not match");
        }

        HttpContext.Session.SetString(Utils.SESSION_KEY, foundUser.Id);

        var currentTime = DateTimeOffset.UtcNow.AddHours(Utils.MAX_JWT_TTL).ToUnixTimeSeconds();
        var payload = new Dictionary<string, object>()
        {
            { "exp", currentTime },
            { "sub", foundUser.Id }
        };
        return Ok(new Dictionary<string, object>()
        {
            { "token", JWT.Encode(payload, ASCIIEncoding.ASCII.GetBytes(Settings.Key), JwsAlgorithm.HS256) }
        });
    }

    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        if (!HttpContext.Session.Keys.Contains(Utils.SESSION_KEY))
        {
            return Unauthorized("no active session");
        }

        HttpContext.Session.Remove(Utils.SESSION_KEY);
        return Ok("logged out");
    }

    [HttpDelete("remove/account")]
    public async Task<IActionResult> DeleteAccount([FromBody] LoginRequest login)
    {
        MongoUser foundUser = await Serv.GetAsync("Username", login.Username);
        if (!BCrypt.Net.BCrypt.Verify(login.Password, foundUser.Password))
        {
            return Unauthorized("invalid user or password");
        }

        await Serv.DeleteAsync(foundUser.Id);
        return Ok("account removed");
    }

    [HttpPost("/allow/notification")]
    [Authorize]
    public async Task<IActionResult> AllowNotification([FromBody] NotificationType[] types)
    {
        var userID = HttpContext.Items[Utils.SESSION_KEY].ToString();
        if (userID is null)
        {
            return BadRequest("invalid token provided");
        }

        var settings = await Serv.GetField<UserSettings>(userID, "Settings");
        if (settings == null)
            return BadRequest("could not complete request");

        if (!settings.AllowNotification)
        {
            return BadRequest("user has notifications disabled");
        }

        bool pushSuccess = await Serv.PushArray(userID, "Settings.AllowedTypes", types);
        if (pushSuccess)
            return Ok("provided notification types have been enabled");
        return BadRequest("could not update allowed notification types");
    }



}

