namespace budz_backend.Models.User;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public RoleType Role { get; set; }
}