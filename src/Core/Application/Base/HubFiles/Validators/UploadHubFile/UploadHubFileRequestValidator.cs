using FluentValidation;
using Shifty.Application.Base.HubFiles.Request.Commands.UploadHubFile;

namespace Shifty.Application.Base.HubFiles.Validators.UploadHubFile;

public class UploadHubFileRequestValidator : AbstractValidator<UploadHubFileRequest>
{
    public UploadHubFileRequestValidator(IStringLocalizer<UploadHubFileRequestValidator> localizer)
    {
        RuleFor(x => x.File).NotNull().WithMessage(localizer["File is required."]);

        RuleFor(x => x.ReportDate).NotEmpty().WithMessage(localizer["Report date is invalid."]);

        RuleFor(x => x.RowType).IsInEnum().WithMessage(localizer["Invalid row type."]);

        RuleFor(x => x.RowId).NotEmpty().WithMessage(localizer["Row ID is required."]);
    }
}