using FluentValidation;
using SmartAttendance.Application.Base.HubFiles.Request.Commands.UploadHubFile;

namespace SmartAttendance.Application.Base.HubFiles.Validators.UploadHubFile;

public class UploadHubFileRequestValidator : AbstractValidator<UploadHubFileRequest>
{
    public UploadHubFileRequestValidator(IStringLocalizer<UploadHubFileRequestValidator> localizer)
    {
        RuleFor(x => x.File).NotNull().WithMessage(localizer["File is required."]);

    }
}