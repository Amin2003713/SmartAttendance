using FluentValidation;
using Shifty.Application.Users.Requests.Commands.ForgotPassword;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.ForgotPassword
{
    /// <summary>
    /// Validates the <see cref="ForgotPasswordRequest"/> ensuring that required fields are present
    /// and follow the appropriate format rules.
    /// </summary>
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordRequestValidator"/> class.
        /// </summary>
        public ForgotPasswordRequestValidator(UserMessages userMessages)
        {
            // Phone number / UserName
            RuleFor(x => x.UserName).
                NotEmpty().
                WithMessage(userMessages.PhoneNumberRequired()).
                Matches(@"^\d{10,}$").
                WithMessage(userMessages.PhoneNumberMinDigits());

            // New Password
            RuleFor(x => x.NewPassword).
                NotEmpty().
                WithMessage(userMessages.PasswordRequired()).
                MinimumLength(6).
                WithMessage(userMessages.PasswordMinLength()).
                Matches(@"[A-Z]").
                WithMessage(userMessages.PasswordUppercaseRequired()).
                Matches(@"[a-z]").
                WithMessage(userMessages.PasswordLowercaseRequired()).
                Matches(@"\d").
                WithMessage(userMessages.PasswordDigitRequired()).
                Matches(@"[\W_]").
                WithMessage(userMessages.PasswordSpecialRequired());

            // Confirm Password
            RuleFor(x => x.ConfirmPassword).
                NotEmpty().
                WithMessage(userMessages.ConfirmPasswordRequired()).
                Equal(x => x.NewPassword).
                WithMessage(userMessages.ConfirmPasswordMustMatch());
        }
    }
}