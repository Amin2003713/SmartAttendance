using SmartAttendance.Application.Features.Documents.Commands;

namespace SmartAttendance.Application.Features.Documents.Validators;

public sealed class RejectDocumentValidator : AbstractValidator<RejectDocumentCommand>
{
    public RejectDocumentValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty().WithMessage("شناسه سند الزامی است.");
    }
}