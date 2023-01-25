namespace budz_backend.Models.Validator;

using FluentValidation;
using budz_backend.Models.User;
using Utils.Consts;
using System.Text.RegularExpressions;
using budz_backend.Models.Notification;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty()
            .MinimumLength(Utils.MIN_PASSWORD_LENGTH)
            .MaximumLength(Utils.MAX_PASSWORD_LENGTH)
            .Matches(Utils.USERNAME_REGEX);


        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(Utils.MIN_PASSWORD_LENGTH)
            .MaximumLength(Utils.MAX_PASSWORD_LENGTH)
            .Matches(Utils.PASSWORD_REGEX);

    }
}


public class NotificationValidator : AbstractValidator<InternalNotification>
{
    public NotificationValidator()
    {
        RuleFor(notification => notification.Title)
        .NotEmpty()
        .MaximumLength(Utils.MAX_NOTIFICATION_TITLE_LEN)
        .MinimumLength(Utils.MIN_NOTIFICATION_TITLE_LEN)
        .Matches(Utils.PASSWORD_REGEX);

        RuleFor(notification => notification.NotificationBody)
            .NotNull()
            .WithMessage("must have a message body")
            .Matches(Utils.PASSWORD_REGEX);
    }
}