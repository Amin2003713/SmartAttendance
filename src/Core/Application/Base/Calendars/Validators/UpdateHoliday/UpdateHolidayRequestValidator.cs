using FluentValidation;
using Shifty.Application.Base.Calendars.Request.Commands.UpdateHoliday;

namespace Shifty.Application.Base.Calendars.Validators.UpdateHoliday;

public class UpdateHolidayRequestValidator : AbstractValidator<UpdateHolidayRequest>
{
    public UpdateHolidayRequestValidator(IStringLocalizer<UpdateHolidayRequestValidator> localizer)
    {
        RuleFor(x => x.HolidayId).NotEmpty().WithMessage(localizer["Holiday ID is required."]);

        RuleFor(x => x.Details)
            .NotEmpty()
            .WithMessage(localizer["ِDetails is required."])
            .Length(1, 255)
            .WithMessage(localizer["Details must be between 1 and 255 characters."]);

        RuleFor(x => x.ProjectId).NotEmpty().WithMessage(localizer["Projects is required."]);

        RuleFor(x => x.Date).NotEmpty().WithMessage(localizer["Date is required."]);
    }
}