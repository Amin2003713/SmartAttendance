using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Shifty.Application.Divisions.Requests.Create;
using Shifty.Domain.Interfaces.Features.Divisions.Commands;
using Shifty.Domain.Interfaces.Features.Divisions.Queries;
using Shifty.Domain.Interfaces.Features.Shifts;
using Shifty.Resources.Messages;

namespace Shifty.Application.Divisions.Validators.Create;

public class CreateDivisionValidator : AbstractValidator<CreateDivisionRequest>
{
    private readonly IDivisionQueriesRepository _divisionRepository;
    private readonly IShiftQueryRepository _shiftRepository;

    public CreateDivisionValidator(
        IDivisionQueriesRepository divisionRepository,
        IShiftQueryRepository shiftRepository,
        DivisionMessages messages)
    {
        _divisionRepository = divisionRepository;
        _shiftRepository = shiftRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(messages.NAME_IS_REQUIRED)
            .MaximumLength(100).WithMessage(messages.NAME_LENGTH);

        RuleFor(x => x.ParentId)
            .NotEqual(Guid.Empty).WithMessage(messages.PARENT_ID_INVALID)
            .Must(ParentExists).WithMessage(messages.PARENT_NOT_FOUND)
            ;

        RuleFor(x => x.ShiftId)
            .NotEqual(Guid.Empty).WithMessage(messages.SHIFT_ID_INVALID)
            .Must(ShiftExists).WithMessage(messages.SHIFT_NOT_FOUND);
    }

    /// <summary>
    /// Validates if the shift ID exists in the database.
    /// </summary>
    private bool ShiftExists(Guid? shiftId)
        => !shiftId.HasValue || // Skip validation if ShiftId is null
           _shiftRepository.Any(a => a.Id == shiftId.Value);

    /// <summary>
    /// Validates if the parent division ID exists in the database.
    /// </summary>
    private bool ParentExists(Guid? parentId)
        => !parentId.HasValue || // Skip validation if ParentId is null
           _divisionRepository.Any(a => a.Id == parentId.Value );
}
