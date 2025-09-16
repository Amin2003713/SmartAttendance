using SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

namespace SmartAttendance.Application.Base.HubFiles.Validators.ZipExport;

public class ZipExportCommandRequestValidator : AbstractValidator<ZipExportCommandRequest>
{
    public ZipExportCommandRequestValidator(IStringLocalizer<ZipExportCommandRequestValidator> localizer)
    {
        RuleFor(x => x.FromDate).NotEmpty().WithMessage(localizer["Start date is invalid."]);

        RuleFor(x => x.ToDate).NotEmpty().WithMessage(localizer["End date is invalid."]);

        RuleFor(x => x).Must(x => x.ToDate >= x.FromDate).WithMessage(localizer["End date must be equal or after Start date."]);

        RuleFor(x => x.RowType).IsInEnum().WithMessage(localizer["Invalid row type."]);
    }
}