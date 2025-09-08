using Mapster;
using SmartAttendance.Application.Features.Departments.Commands.Create;
using SmartAttendance.Application.Interfaces.Departments;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Departments;

namespace SmartAttendance.RequestHandlers.Features.Departments.Commands.Create;

public class CreateDepartmentCommandHandler(
    IDepartmentCommandRepository commandRepository,
    ILogger<CreateDepartmentCommandHandler> logger,
    IStringLocalizer<CreateDepartmentCommandHandler> localizer,
    IDepartmentQueryRepository queryRepository,
    IUserQueryRepository userQueryRepository
) : IRequestHandler<CreateDepartmentCommand>
{
    public async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        logger.LogInformation("Creating department. Title={Title}, ParentId={ParentId}, ManagerId={ManagerId}",
            request.Title,
            request.ParentDepartmentId,
            request.ManagerId);

        try
        {
            if (request.ParentDepartmentId.HasValue)
            {
                var parent = await queryRepository.GetSingleAsync(
                    cancellationToken,
                    x => x.Id == request.ParentDepartmentId.Value);

                if (parent is null)
                    throw SmartAttendanceException.NotFound(localizer["Parent department not found."]);
            }

            if (request.ManagerId.HasValue)
            {
                var manager = await userQueryRepository.GetSingleAsync(
                    cancellationToken,
                    x => x.Id == request.ManagerId.Value);

                if (manager is null)
                    throw SmartAttendanceException.NotFound(localizer["Manager user not found."]);
            }

            var department = request.Adapt<Department>();

            await commandRepository.AddAsync(department, cancellationToken);

            logger.LogInformation("Department created. Id={DepartmentId}, Name={Name}",
                department.Id,
                department.Title);
        }
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error when creating department. Name={Name}", request.Title);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while creating the department."]);
        }
    }
}