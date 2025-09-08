using SmartAttendance.Application.Base.Payment.Request.Queries.ListPayment;

namespace SmartAttendance.Persistence.Repositories.Tenants.Payment;

public class PaymentQueryRepository(
    SmartAttendanceTenantDbContext db,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> tenantContextAccessor,
    ILogger<PaymentQueryRepository> logger,
    IStringLocalizer<PaymentQueryRepository> localizer
) : IPaymentQueryRepository
{
    private SmartAttendanceTenantInfo TenantInfo => tenantContextAccessor.MultiTenantContext.TenantInfo!;

    public async Task<List<PaymentQueryResponse>> ListPayments(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching list of payments for TenantId: {TenantId}", TenantInfo.Id);

            return await db.Payments.Where(a => a.TenantId == TenantInfo.Id && a.Status != 10)
                .ProjectToType<PaymentQueryResponse>()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching payment list for TenantId: {TenantId}", TenantInfo.Id);
            throw new InvalidOperationException(localizer["An error occurred while retrieving payment list."]);
        }
    }

    public async Task<Payments?> GetPayment(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching active payment for TenantId: {TenantId}", TenantInfo.Id);

            return await db.Payments
                .SingleOrDefaultAsync(a => a.TenantId == TenantInfo.Id && a.IsActive,
                    cancellationToken);

            // return await db.Payments.Include(a => a.ActiveServices)
            //     .SingleOrDefaultAsync(a => a.TenantId == TenantInfo.Id && a.IsActive,
            //         cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching active payment for TenantId: {TenantId}", TenantInfo.Id);
            throw new InvalidOperationException(localizer["An error occurred while retrieving active payment."]);
        }
    }

    public async Task<Payments> GetPayment(string authority, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching payment by Authority: {Authority}", authority);

            return await db.Payments.IgnoreQueryFilters()
                       .AsNoTracking()
                       .SingleOrDefaultAsync(a => a.Authority == authority, cancellationToken) ??
                   throw new KeyNotFoundException(localizer["Payment not found with the given authority."]);

            //     return await db.Payments.IgnoreQueryFilters()
            //                .AsNoTracking()
            //                .Include(a => a.ActiveServices)
            //                .SingleOrDefaultAsync(a => a.Authority == authority, cancellationToken) ??
            //            throw new KeyNotFoundException(localizer["Payment not found with the given authority."]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching payment with Authority: {Authority}", authority);
            throw new InvalidOperationException(localizer["An error occurred while retrieving payment by authority."]);
        }
    }

    public async Task<Payments> GetPayment(Guid paymentId, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching payment by Id: {PaymentId}", paymentId);

            return await db.Payments.IgnoreQueryFilters()
                       .AsNoTracking()
                       .SingleOrDefaultAsync(a => a.Id == paymentId, cancellationToken) ??
                   throw new KeyNotFoundException(localizer["Payment not found with the given ID."]);

            // return await db.Payments.IgnoreQueryFilters()
            //            .AsNoTracking()
            //            .Include(a => a.ActiveServices)
            //            .SingleOrDefaultAsync(a => a.Id == paymentId, cancellationToken) ??
            //        throw new KeyNotFoundException(localizer["Payment not found with the given ID."]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching payment with Id: {PaymentId}", paymentId);
            throw new InvalidOperationException(localizer["An error occurred while retrieving payment by ID."]);
        }
    }

    public async Task<Payments> GetPaymentWithSuccess(Guid paymentId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching payment with success by Id: {PaymentId}", paymentId);

            return await db.Payments.IgnoreQueryFilters()
                       .AsNoTracking()
                       .SingleOrDefaultAsync(a => a.Id == paymentId, cancellationToken) ??
                   throw new KeyNotFoundException(localizer["Payment not found with the given ID."]);


            // return await db.Payments.IgnoreQueryFilters()
            //            .AsNoTracking()
            //            .Include(a => a.ActiveServices)
            //            .SingleOrDefaultAsync(a => a.Id == paymentId, cancellationToken) ??
            //        throw new KeyNotFoundException(localizer["Payment not found with the given ID."]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching successful payment with Id: {PaymentId}", paymentId);
            throw new InvalidOperationException(localizer["An error occurred while retrieving payment."]);
        }
    }
}