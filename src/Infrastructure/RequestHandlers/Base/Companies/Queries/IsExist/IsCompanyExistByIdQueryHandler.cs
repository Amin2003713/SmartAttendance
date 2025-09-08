using SmartAttendance.Application.Base.Companies.Queries.IsExist;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Base.Companies.Queries.IsExist;

public record IsCompanyExistByIdQueryHandler(
    ICompanyRepository CompanyRepository,
    ILogger<IsCompanyExistByIdQueryHandler> Logger
)
    : IRequestHandler<IsCompanyExistByIdQuery, bool>
{
    public async Task<bool> Handle(IsCompanyExistByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Checking existence of company with Id: {CompanyId}", request.Id);
            var result = await CompanyRepository.GetByIdAsync(request.Id, cancellationToken);
            var exists = result != null;
            Logger.LogInformation("Company with Id {CompanyId} exists: {Exists}", request.Id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while checking existence of company with Id: {CompanyId}", request.Id);
            throw;
        }
    }
}