using System.Linq;
using FluentValidation;
using Shifty.Application.Users.Requests.Commands.DeleteRole;
using Shifty.Common.General.Enums;
using Shifty.Common.Utilities.TypeConverters;

namespace Shifty.Application.Users.Validators.Commands.DeleteRole;

public class DeleteRoleRequestValidator : AbstractValidator<DeleteRoleRequest>
{
    public DeleteRoleRequestValidator(IStringLocalizer<DeleteRoleRequestValidator> localizer)
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage(localizer["RoleNames are required."])
            .Must(raw =>
            {
                var roles = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return roles.Length > 0 && roles.All(r => r.TryParsToEnum<Roles>());
            })
            .WithMessage(localizer["RoleTypes must be within the allowed role definitions."]);
    }
}