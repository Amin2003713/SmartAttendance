using SmartAttendance.Application.Base.Companies.Commands.AddRequest;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.Persistence.Repositories.Tenants.Companies;

public class CompanyRepository : ICompanyRepository
{
    private readonly SmartAttendanceTenantDbContext      _dbContext;
    private readonly IdentityService                     _identityService;
    private readonly ILogger<CompanyRepository>          _logger;   // Logger instance
    private readonly IStringLocalizer<CompanyRepository> _messages; // Logger instance

    public CompanyRepository(
        SmartAttendanceTenantDbContext      dbContext,
        ILogger<CompanyRepository>          logger,
        IStringLocalizer<CompanyRepository> messages,
        IdentityService                     identityService)
    {
        _dbContext       = dbContext;
        _logger          = logger;
        _messages        = messages;
        _identityService = identityService;
        Entities         = _dbContext.TenantInfo;
    }

    public DbSet<SmartAttendanceTenantInfo> Entities { get; }
    public virtual IQueryable<SmartAttendanceTenantInfo> Table => Entities;
    public virtual IQueryable<SmartAttendanceTenantInfo> TableNoTracking => Entities.AsNoTracking();

    public async Task<bool> IdentifierExistsAsync(string identifier, CancellationToken cancellationToken)
    {
        return await TableNoTracking.AnyAsync(x => x.Identifier == identifier, cancellationToken);
    }


    public async Task<SmartAttendanceTenantInfo> CreateAsync(
        SmartAttendanceTenantInfo tenantInfo,
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
            throw new SmartAttendanceException(additionalData: "Can't create company Server error");
        }
    }

    public async Task CreateAsync(TenantUser tenantUser, CancellationToken cancellationToken)
    {
        try
        {
            if (!await IdentifierExistsAsync(_identityService?.TenantInfo.Identifier!, cancellationToken))
                throw SmartAttendanceException.Conflict(additionalData: _messages["Tenant dose exists."].Value);

            if (tenantUser.SmartAttendanceTenantInfoId == null)
                tenantUser.SmartAttendanceTenantInfoId = _identityService?.TenantInfo?.Id!;

            _dbContext.TenantUsers.Add(tenantUser);


            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Source, e);
            throw new SmartAttendanceException(additionalData: "Can't create company Server error");
        }
    }


    public async Task<bool> ValidateDomain(string domain, CancellationToken cancellationToken)
    {
        return await IdentifierExistsAsync(domain, cancellationToken);
    }

    public Task<SmartAttendanceTenantInfo> GetEntity(
        Expression<Func<SmartAttendanceTenantInfo, bool>> prediction,
        CancellationToken                                 cancellationToken)
    {
        return Entities.SingleOrDefaultAsync(prediction, cancellationToken);
    }

    public Task Update(SmartAttendanceTenantInfo company)
    {
        _dbContext.Entry(company).State = EntityState.Modified;
        return _dbContext.SaveChangesAsync();
    }


    public Task<List<TenantUser>> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.SmartAttendanceTenantInfo)
            .Where(a => a.PhoneNumber == phoneNumber &&
                        (_identityService.TenantInfo == null ||
                         a.SmartAttendanceTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .ToListAsync(cancellationToken)!;
    }

    public Task<TenantUser> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.SmartAttendanceTenantInfo)
            .Where(a => a.Id == userId &&
                        (_identityService.TenantInfo == null ||
                         a.SmartAttendanceTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .FirstOrDefaultAsync(cancellationToken)!;
    }

    public Task<List<TenantUser>> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.SmartAttendanceTenantInfo)
            .Where(a => a.UserName == userName &&
                        (_identityService.TenantInfo == null ||
                         a.SmartAttendanceTenantInfoId.ToString() ==
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


    public async Task<SmartAttendanceTenantInfo> GetByIdAsync(string tenantId, CancellationToken cancellationToken)
    {
        var company = await Table.SingleOrDefaultAsync(a => a.Id == tenantId, cancellationToken);

        if (company == null)
            throw SmartAttendanceException.NotFound(additionalData: "Company not found");

        return company;
    }

    public async Task<IEnumerable<SmartAttendanceTenantInfo>> GetAllAsync(CancellationToken cancellationToken)
    {
        var company = await Table.ToListAsync(cancellationToken);

        if (company.Count == 0)
            throw SmartAttendanceException.NotFound(additionalData: "Company not found");

        return company;
    }

    public async Task<SmartAttendanceTenantInfo> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var company = await Table.SingleOrDefaultAsync(a => a.Identifier == identifier, cancellationToken);

        if (company == null)
            throw SmartAttendanceException.NotFound(additionalData: "Company not found");

        return company;
    }
}