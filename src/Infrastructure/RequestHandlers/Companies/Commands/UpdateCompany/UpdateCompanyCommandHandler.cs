using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Shifty.Application.Companies.Commands.UpdateCompany;
using Shifty.Application.Interfaces.Tenants.Companies;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;

using Shifty.Common.Messaging.Contracts.MinIo.Minio.Commands;
using Shifty.Domain.Tenants;

namespace Shifty.RequestHandlers.Companies.Commands.UpdateCompany;

/// <summary>
///     Handler for UpdateCompanyCommand, implementing IRequestHandler<UpdateCompanyCommand>.
/// </summary>
public class UpdateCompanyCommandHandler(
    IMultiTenantContextAccessor<ShiftyTenantInfo> tenantContextAccessor,
    IStringLocalizer<UpdateCompanyCommandHandler> localizer,
    ICompanyRepository repository,
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

                throw IpaException.NotFound(localizer["The requested company was not found."].Value);
            }

            logger.LogInformation("Updating company details for {CompanyName}", request.Name);

            company.Update(request.Adapt<ShiftyTenantInfo>());

            if (request.Logo == null)
            {
                var path = company.Logo!.Replace("https://", "").Replace("http://", "");
                var deleteResponse = await broker.RequestAsync<DeleteFileResponseBroker, DeleteFileCommandBroker>(
                    new DeleteFileCommandBroker(path),
                    cancellationToken);

                if (!deleteResponse.IsDeleted)
                {
                    logger.LogError("Failed to delete old image for Company {Id}.", company.Id);
                    throw IpaException.InternalServerError(localizer["Failed to delete old image."].Value);
                }
            }


            if (request.Logo?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(company.Logo))
                {
                    var path = company.Logo!.Replace("https://", "").Replace("http://", "");
                    var deleteResponse = await broker.RequestAsync<DeleteFileResponseBroker, DeleteFileCommandBroker>(
                        new DeleteFileCommandBroker(path),
                        cancellationToken);

                    if (!deleteResponse.IsDeleted)
                    {
                        logger.LogError("Failed to delete old image for Company {Id}.", company.Id);
                        throw IpaException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }

                await using var imageStream = new MemoryStream();
                await request.Logo.MediaFile.CopyToAsync(imageStream, cancellationToken);
                var imageBytes = imageStream.ToArray();

                var uploadImageCommand = new UploadHubFileCommandBroker(
                    request.Logo.MediaFile.FileName,
                    imageBytes,
                    null,
                    DateTime.Now,
                    FileStorageType.CompanyLogo,
                    new Guid(company.Id));

                var uploadImageResponse =
                    await broker.RequestAsync<UploadHubFileResponseBroker, UploadHubFileCommandBroker>(
                        uploadImageCommand,
                        cancellationToken);

                company.Logo = uploadImageResponse.FileUrl;

                logger.LogInformation("Uploaded image for Company{CompanyId}.", company.Id);
            }


            await repository.Update(company);

            logger.LogInformation("Successfully updated company {CompanyName} (Identifier: {TenantId})",
                request.Name,
                tenantContextAccessor.MultiTenantContext.TenantInfo!.Identifier);
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business validation error: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating the company.");
            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while processing the request."].Value);
        }
    }
}