using FluentValidation;
using SmartAttendance.Application.Features.Messages.Request.Commands.CreateMessage;

namespace SmartAttendance.Application.Features.Messages.Validators.CreateMessage;

public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>
{
    public CreateMessageRequestValidator(IStringLocalizer<CreateMessageRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(localizer["Title is required."])
            .Length(1, 255)
            .WithMessage(localizer["Title must not exceed 255 characters."]);


        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localizer["Description is required."])
            .Length(1, 10000)
            .WithMessage(localizer["Description must not exceed 10000 characters."]);
    }
}