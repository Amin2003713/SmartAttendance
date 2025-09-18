using SmartAttendance.Application.Base.Universities.Commands.AddRequest;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.Persistence.Repositories.Tenants.Companies;

public class UniversityRepository : IUniversityRepository
{
    private readonly SmartAttendanceTenantDbContext         _dbContext;
    private readonly IdentityService                        _identityService;
    private readonly ILogger<UniversityRepository>          _logger;   // Logger instance
    private readonly IStringLocalizer<UniversityRepository> _messages; // Logger instance

    public UniversityRepository(
        SmartAttendanceTenantDbContext      dbContext,
        ILogger<UniversityRepository>          logger,
        IStringLocalizer<UniversityRepository> messages,
        IdentityService                     identityService)
    {
        _dbContext       = dbContext;
        _logger          = logger;
        _messages        = messages;
        _identityService = identityService;
        Entities         = _dbContext.TenantInfo;
    }

    public DbSet<UniversityTenantInfo> Entities { get; }
    public virtual IQueryable<UniversityTenantInfo> Table => Entities;
    public virtual IQueryable<UniversityTenantInfo> TableNoTracking => Entities.AsNoTracking();

    public async Task<bool> IdentifierExistsAsync(string identifier, CancellationToken cancellationToken)
    {
        return await TableNoTracking.AnyAsync(x => x.Identifier == identifier, cancellationToken);
    }


    public async Task<UniversityTenantInfo> CreateAsync(
        UniversityTenantInfo tenantInfo,
        CancellationToken         cancellationToken,
        bool                      saveNow = true)
    {
        try
        {
            if (await IdentifierExistsAsync(tenantInfo?.Identifier!, cancellationToken))
                throw SmartAttendanceException.Conflict(additionalData: _messages["Tenant already exists."].Value);

            Entities.Add(tenantInfo!);

            if (!saveNow)
                return tenantInfo!;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await TableNoTracking.SingleOrDefaultAsync(x => x.Identifier == tenantInfo!.Identifier,
                       cancellationToken) ??
                   null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Source, e);
            throw new SmartAttendanceException(additionalData: "Can't create University Server error");
        }
    }

    public async Task CreateAsync(UniversityUser UniversityUser, CancellationToken cancellationToken)
    {
        try
        {
            if (!await IdentifierExistsAsync(_identityService?.TenantInfo.Identifier!, cancellationToken))
                throw SmartAttendanceException.Conflict(additionalData: _messages["Tenant dose exists."].Value);

            if (UniversityUser.UniversityTenantInfoId == null)
                UniversityUser.UniversityTenantInfoId = _identityService?.TenantInfo?.Id!;

            _dbContext.UniversityUsers.Add(UniversityUser);


            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Source, e);
            throw new SmartAttendanceException(additionalData: "Can't create University Server error");
        }
    }


    public async Task<bool> ValidateDomain(string domain, CancellationToken cancellationToken)
    {
        return await IdentifierExistsAsync(domain, cancellationToken);
    }

    public Task<UniversityTenantInfo> GetEntity(
        Expression<Func<UniversityTenantInfo, bool>> prediction,
        CancellationToken                                 cancellationToken)
    {
        return Entities.SingleOrDefaultAsync(prediction, cancellationToken);
    }

    public Task Update(UniversityTenantInfo University)
    {
        _dbContext.Entry(University).State = EntityState.Modified;
        return _dbContext.SaveChangesAsync();
    }


    public Task<List<UniversityUser>> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return _dbContext.UniversityUsers.Include(a => a.UniversityTenantInfo)
            .Where(a => a.PhoneNumber == phoneNumber &&
                        (_identityService.TenantInfo == null ||
                         a.UniversityTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .ToListAsync(cancellationToken)!;
    }

    public Task<UniversityUser> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.UniversityUsers.Include(a => a.UniversityTenantInfo)
            .Where(a => a.Id == userId &&
                        (_identityService.TenantInfo == null ||
                         a.UniversityTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .FirstOrDefaultAsync(cancellationToken)!;
    }

    public Task<List<UniversityUser>> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return _dbContext.UniversityUsers.Include(a => a.UniversityTenantInfo)
            .Where(a => a.UserName == userName &&
                        (_identityService.TenantInfo == null ||
                         a.UniversityTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .ToListAsync(cancellationToken)!;
    }

    public async Task AddRequest(AddRequestCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.TenantRequests.AnyAsync(
                a => a.CorrelationId == request.CorrelationId && a.TenantId == request.TenantId,
                cancellationToken))
            return;

        var tenantRequest = new TenantRequest
        {
            Endpoint      = request.EndPoint,
            RequestTime   = DateTime.UtcNow,
            TenantId      = request.TenantId,
            UserId        = request.UserId,
            CorrelationId = request.CorrelationId,
            ServiceName   = request.ServiceName
        };

        _dbContext.TenantRequests.Add(tenantRequest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task<UniversityTenantInfo> GetByIdAsync(string tenantId, CancellationToken cancellationToken)
    {
        var University = await Table.SingleOrDefaultAsync(a => a.Id == tenantId, cancellationToken);

        if (University == null)
            throw SmartAttendanceException.NotFound(additionalData: "University not found");

        return University;
    }

    public async Task<IEnumerable<UniversityTenantInfo>> GetAllAsync(CancellationToken cancellationToken)
    {
        var University = await Table.ToListAsync(cancellationToken);

        if (University.Count == 0)
            throw SmartAttendanceException.NotFound(additionalData: "University not found");

        return University;
    }

    public async Task<UniversityTenantInfo> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var University = await Table.SingleOrDefaultAsync(a => a.Identifier == identifier, cancellationToken);

        if (University == null)
            throw SmartAttendanceException.NotFound(additionalData: "University not found");

        return University;
    }
}