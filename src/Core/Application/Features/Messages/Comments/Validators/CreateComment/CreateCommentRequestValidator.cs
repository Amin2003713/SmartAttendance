using FluentValidation;
using SmartAttendance.Application.Features.Messages.Comments.Request.Commands.CreateComment;

namespace SmartAttendance.Application.Features.Messages.Comments.Validators.CreateComment;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator(IStringLocalizer<CreateCommentRequestValidator> localizer)
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .NotNull()
            .WithMessage(localizer["Comment text is required."])
            .MaximumLength(500)
            .WithMessage(localizer["Comment must not exceed 500 characters."]);

        RuleFor(x => x.MessageId).NotEmpty().WithMessage(localizer["Message ID is required."]);
    }
}