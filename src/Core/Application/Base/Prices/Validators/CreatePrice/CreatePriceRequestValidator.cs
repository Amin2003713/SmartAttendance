using FluentValidation;
using SmartAttendance.Application.Base.Prices.Request.Commands.CreatePrice;

namespace SmartAttendance.Application.Base.Prices.Validators.CreatePrice;

public class CreatePriceRequestValidator : AbstractValidator<CreatePriceRequest>
{
    public CreatePriceRequestValidator(IStringLocalizer<CreatePriceRequestValidator> localizer)
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(localizer["Amount must be greater than zero."]);
    }
}