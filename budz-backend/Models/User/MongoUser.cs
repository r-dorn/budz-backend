

using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using budz_backend.Models.User.Settings;
using MongoDB.Bson.Serialization.Attributes;



namespace budz_backend.Models.User;

public enum RoleType
{
    User,
    Grower
}

public record MongoUser
{
    [Required]
    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; }

    [Required] public string Username { get; set; }

    [Required]
    [BsonElement("password")]
    [JsonIgnore]
    public string Password { get; set; }

    [Required] public bool IsBanned { get; set; } = false;

    [Required] public RoleType Role { get; set; }

    public List<string> OwnedStrains { get; set; }

    public UserSettings Settings = new UserSettings();
    public byte[] ProfilePicture { get; set; } = Array.Empty<byte>();
    public string[] FollowedUsers { get; set; } = { };
    public string[] Followers { get; set; } = { };

    public void Setup()
    {
        Id = Guid.NewGuid().ToString();
        Password = BCrypt.Net.BCrypt.HashPassword(Password);
    }
}
