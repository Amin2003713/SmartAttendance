using FluentValidation;
using SmartAttendance.Application.Commons.MediaFiles.Requests;

namespace SmartAttendance.Application.Commons.MediaFiles.Validators;

public class UploadMediaFileRequestValidator : AbstractValidator<UploadMediaFileRequest>
{
    public UploadMediaFileRequestValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                x == null ||                                                     // optional object
                x.MediaFile != null && string.IsNullOrWhiteSpace(x.MediaUrl) ||  // new upload
                x.MediaFile == null && !string.IsNullOrWhiteSpace(x.MediaUrl) || // reuse existing
                x.MediaFile != null && !string.IsNullOrWhiteSpace(x.MediaUrl))   // delete old, upload new
            .WithMessage("Provide either 'MediaUrl' or 'MediaFile', or both if replacing the existing file.");
    }
}