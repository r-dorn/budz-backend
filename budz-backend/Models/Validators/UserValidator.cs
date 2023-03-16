namespace budz_backend.Models.Validator;

using FluentValidation;
using budz_backend.Models.User;
using Utils.Consts;


public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .MinimumLength(Utils.MIN_PASSWORD_LEN)
            .MaximumLength(Utils.MAX_PASSWORD_LEN)
            .Matches(Utils.USERNAME_REGEX);


        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(Utils.MIN_PASSWORD_LEN).WithMessage("Your password length must be at least 8.")
                .MaximumLength(Utils.MAX_PASSWORD_LEN).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
    }

}


