using System.Linq;
using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;
using SmartAttendance.Common.Utilities.RolesHelper;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.UpdateEmployee;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator(IStringLocalizer<UpdateEmployeeValidator> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserId is required."]);

        RuleFor(x => x.Roles).
            NotNull().
            WithMessage(localizer["RoleTypes are required."]).
            Must(roles => roles.All(RoleParser.IsValid)).
            WithMessage(localizer["RoleTypes must be within the allowed role definitions."]);
    }
}