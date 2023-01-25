using System.Text.RegularExpressions;

namespace budz_backend.Models.User;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public RoleType Role { get; set; }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Password).NotEmpty()
            .Length(min: Utils.Consts.Utils.MIN_PASSWORD_LEN, Utils.Consts.Utils.MAX_PASSWORD_LEN)
            .Custom((password, ValidationContext) =>
            {
                if (!Regex.IsMatch(password, Utils.Consts.Utils.PASSWORD_REGEX))
                    ValidationContext.AddFailure("password does not meet minimum requirements");
            });

        RuleFor(x => x.Username)
            .NotEmpty().Length(min: Utils.Consts.Utils.MIN_PASSWORD_LEN, Utils.Consts.Utils.MAX_PASSWORD_LEN)
            .Custom((username, ctx) =>
            {
                if (!Regex.IsMatch(username, Utils.Consts.Utils.USERNAME_REGEX))
                    ctx.AddFailure("username does not match requirements");
            });
    }
}
