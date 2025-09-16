using System.Linq;
using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.DeleteRole;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.TypeConverters;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.DeleteRole;

public class DeleteRoleRequestValidator : AbstractValidator<DeleteRoleRequest>
{
    public DeleteRoleRequestValidator(IStringLocalizer<DeleteRoleRequestValidator> localizer)
    {
        RuleFor(x => x.Role).
            NotEmpty().
            WithMessage(localizer["RoleNames are required."]).
            Must(raw =>
                 {
                     var roles = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                     return roles.Length > 0 && roles.All(r => r.TryParsToEnum<Roles>());
                 }).
            WithMessage(localizer["RoleTypes must be within the allowed role definitions."]);
    }
}