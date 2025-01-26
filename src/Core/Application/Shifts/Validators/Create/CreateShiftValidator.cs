using FluentValidation;
using Shifty.Resources.Messages;
using System;

public class CreateShiftValidator : AbstractValidator<CreateShiftRequest>
{
    /// <summary>
    /// Validator for <see cref="CreateShiftRequest"/>.
    /// </summary>
    public CreateShiftValidator(ShiftMessages messages)
    {
        // Name is required and should not be empty
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(messages.NAME_IS_REQUIRED)
            .Length(2, 100).WithMessage(messages.NAME_LENGTH);

        // Start time must be earlier than end time
        RuleFor(x => x)
            .Must(x => x.Arrive < x.Leave)
            .WithMessage(messages.LEAVE_EARLIER_THAN_ARRIVE);

        // Grace periods must be non-negative
        RuleFor(x => x.GraceLateArrival)
            .GreaterThanOrEqualTo(TimeSpan.Zero)
            .WithMessage(messages.GRACE_LATE_ARRIVAL_NEGATIVE);

        RuleFor(x => x.GraceEarlyLeave)
            .GreaterThanOrEqualTo(TimeSpan.Zero)
            .WithMessage(messages.GRACE_EARLY_LEAVE_NEGATIVE);
    }
}