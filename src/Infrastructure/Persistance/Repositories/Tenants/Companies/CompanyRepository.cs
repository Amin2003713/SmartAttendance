using Shifty.Application.Companies.Commands.AddRequest;
using Shifty.Application.Interfaces.Tenants.Companies;

namespace Shifty.Persistence.Repositories.Tenants.Companies;

public class CompanyRepository : ICompanyRepository
{
    private readonly ShiftyTenantDbContext               _dbContext;
    private readonly IdentityService                     _identityService;
    private readonly ILogger<CompanyRepository>          _logger;   // Logger instance
    private readonly IStringLocalizer<CompanyRepository> _messages; // Logger instance

    public CompanyRepository(
        ShiftyTenantDbContext dbContext,
        ILogger<CompanyRepository> logger,
        IStringLocalizer<CompanyRepository> messages,
        IdentityService identityService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _messages = messages;
        _identityService = identityService;
        Entities = _dbContext.TenantInfo;
    }

    public DbSet<ShiftyTenantInfo> Entities { get; }
    public virtual IQueryable<ShiftyTenantInfo> Table => Entities;
    public virtual IQueryable<ShiftyTenantInfo> TableNoTracking => Entities.AsNoTracking();

    public async Task<bool> IdentifierExistsAsync(string identifier, CancellationToken cancellationToken)
    {
        return await TableNoTracking.AnyAsync(x => x.Identifier == identifier, cancellationToken);
    }


    public async Task<ShiftyTenantInfo> CreateAsync(
        ShiftyTenantInfo tenantInfo,
        CancellationToken cancellationToken,
        bool saveNow = true)
    {
        try
        {
            if (await IdentifierExistsAsync(tenantInfo?.Identifier!, cancellationToken))
                throw IpaException.Conflict(additionalData: _messages["Tenant already exists."].Value);

            Entities.Add(tenantInfo);

            if (!saveNow)
                return tenantInfo;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return await TableNoTracking.SingleOrDefaultAsync(x => x.Identifier == tenantInfo.Identifier,
                       cancellationToken) ??
                   null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Source, e);
            throw new IpaException(additionalData: "Can't create company Server error");
        }
    }

    public async Task CreateAsync(TenantUser tenantUser, CancellationToken cancellationToken)
    {
        try
        {
            if (!await IdentifierExistsAsync(_identityService?.TenantInfo.Identifier!, cancellationToken))
                throw IpaException.Conflict(additionalData: _messages["Tenant dose exists."].Value);

            if (tenantUser.ShiftyTenantInfoId == null)
                tenantUser.ShiftyTenantInfoId = _identityService?.TenantInfo?.Id!;

            _dbContext.TenantUsers.Add(tenantUser);


            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Source, e);
            throw new IpaException(additionalData: "Can't create company Server error");
        }
    }


    public async Task<bool> ValidateDomain(string domain, CancellationToken cancellationToken)
    {
        return await IdentifierExistsAsync(domain, cancellationToken);
    }

    public Task<ShiftyTenantInfo> GetEntity(
        Expression<Func<ShiftyTenantInfo, bool>> prediction,
        CancellationToken cancellationToken)
    {
        return Entities.SingleOrDefaultAsync(prediction, cancellationToken);
    }

    public Task Update(ShiftyTenantInfo company)
    {
        _dbContext.Entry(company).State = EntityState.Modified;
        return _dbContext.SaveChangesAsync();
    }


    public Task<List<TenantUser>> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.ShiftyTenantInfo)
            .Where(a => a.PhoneNumber == phoneNumber &&
                        (_identityService.TenantInfo == null ||
                         a.ShiftyTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .ToListAsync(cancellationToken)!;
    }

    public Task<TenantUser> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.ShiftyTenantInfo)
            .Where(a => a.Id == userId &&
                        (_identityService.TenantInfo == null ||
                         a.ShiftyTenantInfoId.ToString() ==
                         _identityService.TenantInfo.Id))
            .FirstOrDefaultAsync(cancellationToken)!;
    }

    public Task<List<TenantUser>> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return _dbContext.TenantUsers.Include(a => a.ShiftyTenantInfo)
            .Where(a => a.UserName == userName &&
                        (_identityService.TenantInfo == null ||
                         a.ShiftyTenantInfoId.ToString() ==
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
            Endpoint = request.EndPoint,
            RequestTime = DateTime.UtcNow,
            TenantId = request.TenantId,
            UserId = request.UserId,
            CorrelationId = request.CorrelationId,
            ServiceName = request.ServiceName
        };

        _dbContext.TenantRequests.Add(tenantRequest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task<ShiftyTenantInfo> GetByIdAsync(string tenantId, CancellationToken cancellationToken)
    {
        var company = await Table.SingleOrDefaultAsync(a => a.Id == tenantId, cancellationToken);

        if (company == null)
            throw IpaException.NotFound(additionalData: "Company not found");

        return company;
    }

    public async Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken)
    {
        var company = await Table.ToListAsync(cancellationToken);

        if (company.Count == 0)
            throw IpaException.NotFound(additionalData: "Company not found");

        return company;
    }

    public async Task<ShiftyTenantInfo> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        var company = await Table.SingleOrDefaultAsync(a => a.Identifier == identifier, cancellationToken);

        if (company == null)
            throw IpaException.NotFound(additionalData: "Company not found");

        return company;
    }
}