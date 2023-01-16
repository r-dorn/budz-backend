namespace budz_backend.Models.Validator;

using FluentValidation;
using budz_backend.Models.User;
using Utils.Consts;
using System.Text.RegularExpressions;
using budz_backend.Models.Notification;

public class MongoUserValidator : AbstractValidator<MongoUser>
{
    public MongoUserValidator()
    {
        RuleFor(user => user.Username)
            .NotNull()
            .Length(Utils.MAX_USERNAME_LENGTH)
            .WithMessage($"Username cannot be over {Utils.MAX_USERNAME_LENGTH} characters")
            .Custom((username, context) =>
            {
                if (!Regex.IsMatch(username, Utils.USERNAME_REGEX))
                {
                    context.AddFailure("user can only contain letters, numbers or a hyphen");
                }
            });


    }
}


public class NotificationValidator : AbstractValidator<Notification>
{
    public NotificationValidator()
    {
        RuleFor(notification)
    }
}