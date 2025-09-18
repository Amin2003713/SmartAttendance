using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Base.Universities.Commands.UpdateUniversity;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.RequestHandlers.Base.Universites.Commands.UpdateUniversity;

/// <summary>
///     Handler for UpdateUniversityCommand, implementing IRequestHandler<UpdateUniversityCommand>.
/// </summary>
public class UpdateUniversityCommandHandler(
    IMultiTenantContextAccessor<UniversityTenantInfo> tenantContextAccessor,
    IStringLocalizer<UpdateUniversityCommandHandler>          localizer,
    IUniversityRepository                                     repository,
    IMediator                                              mediator,
    ILogger<UpdateUniversityCommandHandler>                   logger
) : IRequestHandler<UpdateUniversityCommand>
{
    public async Task Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            logger.LogInformation("Starting update process for University with identifier: {TenantId}",
                tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);

            var University = await repository.GetEntity(
                info => info.Identifier == tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier,
                cancellationToken
            );

            if (University == null)
            {
                logger.LogWarning("University not found for identifier: {TenantId}",
                    tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);

                throw SmartAttendanceException.NotFound(localizer["The requested University was not found."].Value);
            }

            logger.LogInformation("Updating University details for {UniversityName}", request.Name);

            University.Update(request.Adapt<UniversityTenantInfo>());

            if (request.Logo == null)
            {
                var path = University.Logo!.Replace("https://", "").Replace("http://", "");

                var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                    cancellationToken);

                if (!deleteResponse)
                {
                    logger.LogError("Failed to delete old image for University {Id}.", University.Id);
                    throw SmartAttendanceException.InternalServerError(localizer["Failed to delete old image."].Value);
                }
            }


            if (request.Logo?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(University.Logo))
                {
                    var path = University.Logo!.Replace("https://", "").Replace("http://", "");

                    var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                        cancellationToken);

                    if (!deleteResponse)
                    {
                        logger.LogError("Failed to delete old image for University {Id}.", University.Id);
                        throw SmartAttendanceException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommand
                {
                    File       = request.Logo.MediaFile,
                    ReportDate = DateTime.UtcNow,
                    RowType    = FileStorageType.UniversityLogo,
                    RowId      = new Guid(University.Id)
                };

                var uploadImageResponse = await mediator.Send(uploadCommand, cancellationToken);

                University.Logo = uploadImageResponse.Url;

                logger.LogInformation("Uploaded image for University{UniversityId}.", University.Id);
            }


            await repository.Update(University);

            logger.LogInformation("Successfully updated University {UniversityName} (Identifier: {TenantId})",
                request.Name,
                tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business validation error: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating the University.");

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while processing the request."].Value);
        }
    }
}