using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Shifty.Application.Base.Companies.Commands.UpdateCompany;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.MinIo.Commands.DeleteFile;
using Shifty.Application.Interfaces.Tenants.Companies;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;
using Shifty.Domain.Tenants;

namespace Shifty.RequestHandlers.Base.Companies.Commands.UpdateCompany;

/// <summary>
///     Handler for UpdateCompanyCommand, implementing IRequestHandler<UpdateCompanyCommand>.
/// </summary>
public class UpdateCompanyCommandHandler(
    IMultiTenantContextAccessor<ShiftyTenantInfo> tenantContextAccessor,
    IStringLocalizer<UpdateCompanyCommandHandler> localizer,
    ICompanyRepository repository,
    IMediator mediator,
    ILogger<UpdateCompanyCommandHandler> logger
) : IRequestHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            logger.LogInformation("Starting update process for company with identifier: {TenantId}",
                tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);

            var company = await repository.GetEntity(
                info => info.Identifier == tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier,
                cancellationToken
            );

            if (company == null)
            {
                logger.LogWarning("Company not found for identifier: {TenantId}",
                    tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);

                throw ShiftyException.NotFound(localizer["The requested company was not found."].Value);
            }

            logger.LogInformation("Updating company details for {CompanyName}", request.Name);

            company.Update(request.Adapt<ShiftyTenantInfo>());

            if (request.Logo == null)
            {
                var path = company.Logo!.Replace("https://", "").Replace("http://", "");
                var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                    cancellationToken);

                if (!deleteResponse)
                {
                    logger.LogError("Failed to delete old image for Company {Id}.", company.Id);
                    throw ShiftyException.InternalServerError(localizer["Failed to delete old image."].Value);
                }
            }


            if (request.Logo?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(company.Logo))
                {
                    var path = company.Logo!.Replace("https://", "").Replace("http://", "");
                    var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                        cancellationToken);

                    if (!deleteResponse)
                    {
                        logger.LogError("Failed to delete old image for Company {Id}.", company.Id);
                        throw ShiftyException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommand
                {
                    File = request.Logo.MediaFile,
                    ReportDate = DateTime.UtcNow,
                    RowType = FileStorageType.CompanyLogo,
                    RowId = new Guid(company.Id)
                };

                var uploadImageResponse = await mediator.Send(uploadCommand, cancellationToken);

                company.Logo = uploadImageResponse.Url;

                logger.LogInformation("Uploaded image for Company{CompanyId}.", company.Id);
            }


            await repository.Update(company);

            logger.LogInformation("Successfully updated company {CompanyName} (Identifier: {TenantId})",
                request.Name,
                tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business validation error: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating the company.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while processing the request."].Value);
        }
    }
}