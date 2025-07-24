using FluentValidation;
using Shifty.Application.Payment.Request.Commands.CreatePayment;

namespace Shifty.Application.Payment.Validators.CreatePayment;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator(IStringLocalizer<CreatePaymentRequestValidator> localizer)
    {
        RuleFor(x => x.UsersCount).GreaterThan(0).WithMessage(localizer["Users count must be greater than zero."]);

        RuleFor(x => x.PaymentStatus).IsInEnum().WithMessage(localizer["Invalid payment status."]);
    }
}