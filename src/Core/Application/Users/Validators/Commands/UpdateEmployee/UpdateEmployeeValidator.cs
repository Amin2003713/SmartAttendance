using System.Linq;
using FluentValidation;
using Shifty.Application.Users.Requests.Commands.AddRole;
using Shifty.Common.Utilities.RolesHelper;

namespace Shifty.Application.Users.Validators.Commands.UpdateEmployee;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator(IStringLocalizer<UpdateEmployeeValidator> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserId is required."]);

        RuleFor(x => x.Roles)
            .NotNull()
            .WithMessage(localizer["Roles are required."])
            .Must(roles => roles.All(RoleParser.IsValid))
            .WithMessage(localizer["Roles must be within the allowed role definitions."]);
    }
}