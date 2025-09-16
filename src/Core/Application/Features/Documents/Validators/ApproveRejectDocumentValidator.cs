using SmartAttendance.Application.Features.Documents.Commands;

namespace SmartAttendance.Application.Features.Documents.Validators;

public sealed class ApproveDocumentValidator : AbstractValidator<ApproveDocumentCommand>
{
	public ApproveDocumentValidator()
	{
		RuleFor(x => x.DocumentId).NotEmpty().WithMessage("شناسه سند الزامی است.");
	}
}

public sealed class RejectDocumentValidator : AbstractValidator<RejectDocumentCommand>
{
	public RejectDocumentValidator()
	{
		RuleFor(x => x.DocumentId).NotEmpty().WithMessage("شناسه سند الزامی است.");
	}
}

