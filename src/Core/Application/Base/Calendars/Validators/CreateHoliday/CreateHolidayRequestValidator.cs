using FluentValidation;
using Shifty.Application.Base.Calendars.Request.Commands.CreateHoliday;

namespace Shifty.Application.Base.Calendars.Validators.CreateHoliday;

public class CreateHolidayRequestValidator : AbstractValidator<CreateHolidayRequest>
{
    public CreateHolidayRequestValidator(IStringLocalizer<CreateHolidayRequestValidator> localizer)
    {
        RuleFor(x => x.Details)
            .NotEmpty()
            .WithMessage(localizer["ِDetails is required."])
            .Length(1, 255)
            .WithMessage(localizer["Details must be between 1 and 255 characters."]);



        RuleFor(x => x.Date).NotEmpty().WithMessage(localizer["Date is required."]);
    }
}