using System.Linq;
using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.UpdateEmployee;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeValidator(IStringLocalizer<UpdateEmployeeValidator> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserId is required."]);

    }
}