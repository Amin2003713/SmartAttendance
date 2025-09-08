using System.Linq;
using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;
using SmartAttendance.Common.Utilities.RolesHelper;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.RegisterByOwner;

public class RegisterByOwnerRequestValidator : AbstractValidator<RegisterByOwnerRequest>
{
    public RegisterByOwnerRequestValidator(IStringLocalizer<RegisterByOwnerRequestValidator> localizer)
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizer["FirstName is required."]);

        RuleFor(x => x.LastName).NotEmpty().WithMessage(localizer["LastName is required."]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumber is required."])
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage(localizer["PhoneNumber format is invalid."]);

        RuleFor(x => x.Roles)
            .NotNull()
            .WithMessage(localizer["At least one role is required."])
            .When(a => a.Roles.Count != 0)
            .Must(roles => roles.All(RoleParser.IsValid))
            .WithMessage(localizer["RoleTypes must be within the allowed role definitions."]);
    }
}