using MD.PersianDateTime.Standard;
using Riviera.ZarinPal.V4.Models;
using SmartAttendance.Application.Base.Payment.Commands.CreatePayment;
using SmartAttendance.Application.Base.Payment.Request.Commands.CreatePayment;
using SmartAttendance.Application.Base.ZarinPal.Request;
using SmartAttendance.Application.Interfaces.ZarinPal;
using SmartAttendance.Common.General.Enums.Discount;
using SmartAttendance.Common.General.Enums.Payment;

namespace SmartAttendance.Persistence.Repositories.Tenants.Payment;

public class PaymentCommandRepository(
    SmartAttendanceTenantDbContext db,
    IdentityService service,
    UserManager<User> userManager,
    IZarinPal zarinPal,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> tenantContextAccessor,
    ILogger<PaymentCommandRepository> logger,
    IStringLocalizer<PaymentCommandRepository> localizer
)
    : IPaymentCommandRepository
{
    private SmartAttendanceTenantInfo TenantInfo => tenantContextAccessor.MultiTenantContext.TenantInfo!;

    public async Task<Uri?> CreatePayment(CreatePaymentCommand createPayment, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId();
        logger.LogInformation("Start CreatePayment by UserId: {UserId} for Tenant: {TenantId}", userId, TenantInfo.Id);

        if (!TenantInfo!.IsCompanyRegistrationCompleted())
            throw SmartAttendanceException.BadRequest(localizer["Company registration is not completed."]);

        var user = await userManager.FindByIdAsync(userId.ToString()!);

        var monthlyPrice = await db.Prices.FirstOrDefaultAsync(a => a.IsActive, cancellationToken);

        var lastPurchase = await db.Set<Payments>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.IsActive && a.TenantId == TenantInfo!.Id, cancellationToken);

        if (!IsUserCountValid(lastPurchase, createPayment))
            throw SmartAttendanceException.BadRequest(localizer["You have reached the user limit. Contact support."]);

        var baseCost = BaseCost(createPayment, monthlyPrice, lastPurchase);

        if (IsUserCountValid(createPayment, lastPurchase))
            throw SmartAttendanceException.BadRequest(localizer["You cannot add fewer users than your active subscription."]);

        var price = await Price(createPayment, baseCost, cancellationToken);

        Payments         newCompanyPurch;
        ZarinPalResponse zarinResponse = null!;

        try
        {
            switch (createPayment.PaymentStatus)
            {
                case PaymentType.RenewSubscriptionEarly:
                    newCompanyPurch =
                        CreateCompaniesPurchases(createPayment, price, lastPurchase!, monthlyPrice.Id, user);

                    var earlyParam = NewPayment.Create((int)newCompanyPurch.Cost,
                        $"تمدید {TenantInfo!.Name} تعداد  {createPayment.UsersCount} کاربر ...",
                        TenantInfo.GetUrl()!,
                        user.Email!,
                        user.PhoneNumber!);

                    zarinResponse = await zarinPal.CreatePaymentRequest(earlyParam, cancellationToken);
                    break;

                case PaymentType.RenewSubscription:
                    newCompanyPurch =
                        CreateCompaniesPurchases(createPayment, price, lastPurchase!, monthlyPrice!.Id, user!);

                    var renewParam = NewPayment.Create((int)newCompanyPurch.Cost,
                        $"تمدید {TenantInfo!.Name} تعداد  {createPayment.UsersCount} کاربر ...",
                        TenantInfo.GetUrl()!,
                        user.Email!,
                        user.PhoneNumber!);

                    zarinResponse = await zarinPal.CreatePaymentRequest(renewParam, cancellationToken);
                    break;

                case PaymentType.AddCompanyUser:
                    newCompanyPurch =
                        CreateCompaniesPurchases(createPayment, price, lastPurchase!, monthlyPrice!.Id, user!);

                    var addUserParam = NewPayment.Create((int)price.finalPrice,
                        $"افزایش کاربر برای شرکت {TenantInfo!.Name} ...",
                        TenantInfo.GetUrl()!,
                        user.Email!,
                        user.PhoneNumber!);

                    zarinResponse = await zarinPal.CreatePaymentRequest(addUserParam, cancellationToken);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!zarinResponse.Success)
            {
                logger.LogWarning("ZarinPal request failed for TenantId: {TenantId}", TenantInfo.Id);
                throw SmartAttendanceException.BadRequest(localizer["ZarinPal gateway error. Please contact support."]);
            }

            newCompanyPurch.Authority = zarinResponse.Authority;
            newCompanyPurch.IsActive = false;

            db.Payments.Add(newCompanyPurch);
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Payment created successfully for TenantId: {TenantId}, PaymentId: {PaymentId}",
                TenantInfo.Id,
                newCompanyPurch.Id);

            return zarinResponse.PaymentUri;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating payment for TenantId: {TenantId}", TenantInfo.Id);
            throw;
        }
    }

    public async Task Update(Payments payments, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating payment with ID: {PaymentId}", payments.Id);
            db.Payments.Update(payments);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Payment updated successfully: {PaymentId}", payments.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating payment: {PaymentId}", payments.Id);
            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while updating the payment."]);
        }
    }

    private bool IsUserCountValid(Payments? lastPurchase, CreatePaymentRequest model)
    {
        var isValid = model.PaymentStatus switch
                      {
                          PaymentType.AddCompanyUser         => lastPurchase!.UsersCount + model.UsersCount <= 30,
                          PaymentType.RenewSubscriptionEarly => true,
                          PaymentType.RenewSubscription      => model.UsersCount <= 30, _ => true
                      };

        logger.LogDebug(
            "Validation result for IsUserCountValid: {IsValid}, UsersCount: {UsersCount}, LastPurchaseCount: {LastUsers}",
            isValid,
            model.UsersCount,
            lastPurchase?.UsersCount);

        return isValid;
    }

    private bool IsUserCountValid(CreatePaymentRequest model, Payments? lastPurchase)
    {
        if (lastPurchase is null)
        {
            logger.LogWarning("User count validation failed because no previous purchase exists.");
            return false;
        }

        var isValid = model.UsersCount < lastPurchase.ActiveUsers && model.PaymentStatus != PaymentType.AddCompanyUser;
        logger.LogDebug(
            "Validation result for reducing users: {IsValid}, NewUsers: {UsersCount}, ActiveUsers: {ActiveUsers}",
            isValid,
            model.UsersCount,
            lastPurchase.ActiveUsers);

        return isValid;
    }

    private static double BaseCost(CreatePaymentRequest model, Price? monthlyPrice, Payments? lastPurchase)
    {
        return model.Duration switch
               {
                   12 => BasePrice(model, monthlyPrice, 10), 6 => BasePrice(model, monthlyPrice, 5),
                   3  => BasePrice(model, monthlyPrice, 3),
                   1  => BasePrice(model, monthlyPrice, 1),
                   null => BasePrice(model,
                       monthlyPrice,
                       (float)lastPurchase!.LeftDays() / new PersianDateTime(DateTime.UtcNow).GetMonthDays),
                   _ => throw new ArgumentOutOfRangeException()
               };
    }

    private static double BasePrice(CreatePaymentRequest model, Price? monthlyPrice, float? duration)
    {
        return (double)(monthlyPrice!.Amount * (decimal)(duration ?? 0.00) * model.UsersCount);
    }

    private async Task<(double finalPrice, double taxAmount, double discountAmount, double baseCost)> Price(
        CreatePaymentRequest model,
        double baseCost,
        CancellationToken cancellationToken)
    {
        var discount        = await DiscountPercentage(model, cancellationToken);
        var amount          = baseCost * (discount.percentage ?? 0) / 100 + (discount.price ?? 0);
        var discountedPrice = baseCost - amount;
        var taxedPrice      = discountedPrice * 1.10;

        logger.LogInformation("Price calculated. Base: {Base}, Discount: {Discount}, Final: {Final}",
            baseCost,
            amount,
            taxedPrice);

        return (Math.Floor(taxedPrice), discountedPrice * 0.10, amount, baseCost);
    }

    private async Task<(int? percentage, long? price)> DiscountPercentage(
        CreatePaymentRequest model,
        CancellationToken cancellationToken)
    {
        Discount discount;
        var      userDiscount = Math.Min(model.UsersCount / 5 * 5, 30);

        if (model.DiscountId is not null)
        {
            var dis = await db.Discounts.Include(a => a.TenantDiscount)
                .Include(a => a.Payments)
                .ThenInclude(a => a.Tenant)
                .FirstOrDefaultAsync(a => a.Id == model.DiscountId, cancellationToken);

            if (dis is null)
                throw SmartAttendanceException.NotFound(localizer["Discount not found."]);

            if (dis.PackageMonth is not 0 && dis.PackageMonth != model.Duration)
                throw SmartAttendanceException.BadRequest(localizer["This discount is not valid for your selected duration."]);

            var discountCompany =
                db.TenantDiscounts.FirstOrDefault(p => p.DiscountId == dis.Id && p.TenantId == TenantInfo.Id);

            if (discountCompany is null)
            {
                db.TenantDiscounts.Add(new TenantDiscount
                {
                    TenantId = TenantInfo.Id!,
                    IsUsed = false,
                    DiscountId = dis.Id
                });

                await db.SaveChangesAsync(cancellationToken);
            }

            if (discountCompany is not null && discountCompany.IsUsed)
                throw SmartAttendanceException.BadRequest(localizer["This discount has already been used."]);

            discount = dis;
        }
        else if (userDiscount > 0)
        {
            discount = new Discount
            {
                Code = $"Discount for Each 5 User 5 % for {TenantInfo.Id}",
                Duration = 1,
                StartDate = DateTime.UtcNow,
                Value = userDiscount,
                DiscountType = DiscountType.Percent,
                TenantDiscount =
                [
                    new TenantDiscount
                    {
                        TenantId = TenantInfo.Id!,
                        IsUsed = true
                    }
                ]
            };

            await db.AddAsync(discount, cancellationToken);
        }
        else
        {
            return (0, 0);
        }

        model.DiscountId = discount.Id;
        return ((int? percentage, long? price))(discount.DiscountType == DiscountType.Percent ? discount.Value : 0,
                                                discount.DiscountType == DiscountType.FixedAmount ? discount.Value : 0);
    }

    private Payments CreateCompaniesPurchases(
        CreatePaymentRequest model,
        (double finalPrice, double taxAmount, double discountAmount, double baseCost) cost,
        Payments oldCompany,
        Guid priceId,
        User user)
    {
        logger.LogInformation("Creating purchase entry for TenantId: {TenantId}, PaymentType: {Type}",
            TenantInfo.Id,
            model.PaymentStatus);

        var userCount = model.PaymentStatus switch
                        {
                            PaymentType.RenewSubscriptionEarly => model.UsersCount, PaymentType.RenewSubscription => model.UsersCount,
                            PaymentType.AddCompanyUser         => oldCompany.UsersCount + model.UsersCount,
                            _                                  => throw new ArgumentOutOfRangeException()
                        };

        var startDate = model.PaymentStatus switch
                        {
                            PaymentType.RenewSubscriptionEarly => oldCompany.StartDate, PaymentType.RenewSubscription => DateTime.UtcNow,
                            PaymentType.AddCompanyUser         => oldCompany.StartDate, _                             => throw new ArgumentOutOfRangeException()
                        };

        var endDate = model.PaymentStatus switch
                      {
                          PaymentType.RenewSubscriptionEarly => new PersianDateTime(oldCompany.EndDate).AddMonths(model.Duration ?? 0)
                              .ToDateTime(),
                          PaymentType.RenewSubscription => new PersianDateTime(DateTime.UtcNow).AddMonths(model.Duration ?? 0)
                              .ToDateTime(),
                          PaymentType.AddCompanyUser => oldCompany.EndDate.AddYears(3024),
                          _                          => throw new ArgumentOutOfRangeException()
                      };

        var companyCost = (decimal)cost.finalPrice;

        return new Payments
        {
            Email = oldCompany!.Email!,
            TenantId = oldCompany.TenantId,
            UserId = service.GetUserId<Guid>(),
            Cost = companyCost,
            PhoneNumber = user.PhoneNumber!,
            IsActive = false,
            StartDate = startDate,
            EndDate = endDate,
            Duration = model.Duration ?? oldCompany.Duration,
            UsersCount = userCount,
            PaymentType = model.PaymentStatus,
            Tenant = oldCompany.Tenant,
            DiscountId = model.DiscountId,
            PaymentDate = DateTime.UtcNow,
            ActiveUsers = oldCompany.ActiveUsers,
            LastPaymentId = oldCompany.Id,
            BasePrice = (decimal)cost.baseCost,
            TaxAmount = (decimal)cost.taxAmount,
            DiscountAmount = (decimal)cost.discountAmount,
            PriceId = priceId
        };
    }
}