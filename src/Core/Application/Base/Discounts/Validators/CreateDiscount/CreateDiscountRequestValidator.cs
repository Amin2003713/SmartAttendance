using FluentValidation;
using SmartAttendance.Application.Base.Discounts.Request.Commands.CreateDisCount;

namespace SmartAttendance.Application.Base.Discounts.Validators.CreateDiscount;

public class CreateDiscountRequestValidator : AbstractValidator<CreateDiscountRequest>
{
    public CreateDiscountRequestValidator(IStringLocalizer<CreateDiscountRequestValidator> localizer)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localizer["Code is required."])
            .MaximumLength(100)
            .WithMessage(localizer["Code must not exceed 100 characters."]);

        RuleFor(x => x.StartDate).NotEmpty().WithMessage(localizer["Start date is required."]);

        RuleFor(x => x.Duration).GreaterThan(0).WithMessage(localizer["Duration must be greater than zero."]);

        RuleFor(x => x.DiscountType).IsInEnum().WithMessage(localizer["Invalid discount type."]);

        RuleFor(x => x.Value).GreaterThan(0).WithMessage(localizer["Discount value must be greater than zero."]);

        RuleForEach(x => x.TenantIds).NotEmpty().WithMessage(localizer["Tenant ID must not be empty."]);
    }
}