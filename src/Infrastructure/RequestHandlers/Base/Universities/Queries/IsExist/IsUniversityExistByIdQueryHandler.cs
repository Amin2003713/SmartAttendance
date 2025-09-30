using SmartAttendance.Application.Base.Universities.Queries.IsExist;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Base.Universities.Queries.IsExist;

public record IsUniversityExistByIdQueryHandler(
    IUniversityRepository                      UniversityRepository,
    ILogger<IsUniversityExistByIdQueryHandler> Logger
)
    : IRequestHandler<IsUniversityExistByIdQuery, bool>
{
    public async Task<bool> Handle(IsUniversityExistByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Checking existence of University with Id: {UniversityId}", request.Id);
            var result = await UniversityRepository.GetByIdAsync(request.Id, cancellationToken);
            var exists = result != null;
            Logger.LogInformation("University with Id {UniversityId} exists: {Exists}", request.Id, exists);
            return exists;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while checking existence of University with Id: {UniversityId}", request.Id);
            throw;
        }
    }
}