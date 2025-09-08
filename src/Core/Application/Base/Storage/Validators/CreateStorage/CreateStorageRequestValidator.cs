using FluentValidation;
using SmartAttendance.Application.Base.Storage.Request.Commands.CreateStorage;

namespace SmartAttendance.Application.Base.Storage.Validators.CreateStorage;

public class CreateStorageRequestValidator : AbstractValidator<CreateStorageRequest>
{
    public CreateStorageRequestValidator(IStringLocalizer<CreateStorageRequestValidator> localizer)
    {
        RuleFor(x => x.RowId).NotEmpty().WithMessage(localizer["Row ID is required."]);


        RuleFor(x => x.FileType).IsInEnum().WithMessage(localizer["Invalid file type."]);

        RuleFor(x => x.StorageUsedByItemMb)
            .GreaterThan(0)
            .WithMessage(localizer["Storage value must be greater than zero."]);
    }
}