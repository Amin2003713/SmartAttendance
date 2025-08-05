using Shifty.Domain.Tenants.Payments;

namespace Shifty.Domain.Defaults;

public abstract class TenantDefaultValue
{
    public static Setting.Setting Setting()
    {
        return new Setting.Setting
        {
            Id = Guid.Parse("A360ED40-C440-4258-BF8A-D78B71AD390C"),
            Flags = SettingFlags.CompanyEnabled.AddFlag(SettingFlags.InitialStepper)
        };
    }


    public static Payments DemoPayment(User user, ShiftyTenantInfo info)
    {
        return new Payments
        {
            IsActive = true,
            PaymentDate = DateTime.UtcNow,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(15),
            UsersCount = 10,
            ActiveUsers = 1,
            ProjectsCount = 5,
            Cost = 0,
            BasePrice = 0,
            DiscountAmount = 0,
            TaxAmount = 0,
            GrantedStorageMb = ApplicationConstant.Const.GrantedStorage,
            Status =
#if DEBUG
                100
#else
                10
#endif
           ,
            PaymentType = PaymentType.DemoSubscription,
            Duration = 0,
            PhoneNumber = user.PhoneNumber!,
            Email = user.Email ?? $"{user.PhoneNumber}@gmail.com",
            UserId = user.Id,
            Authority = "Demo",
            RefId = "Demo",
            TenantId = info.Id!,
            CreatedBy = user.Id,
            LastPaymentId = null
        };
    }


    public static List<ActiveService> CreateActiveService(Payments payment)
    {
        return
        [
            new ActiveService
            {
                Id = Guid.NewGuid(),
                Name = "prima",
                PaymentId = payment.Id,
                Price = 1611
            },
            new ActiveService
            {
                Id = Guid.NewGuid(),
                Name = "tenant",
                PaymentId = payment.Id,
                Price = 1611
            },
            new ActiveService
            {
                Id = Guid.NewGuid(),
                Name = "msp",
                PaymentId = payment.Id,
                Price = 1611
            },
            new ActiveService
            {
                Id = Guid.NewGuid(),
                Name = "taskTracker",
                PaymentId = payment.Id,
                Price = 1611
            },
            new ActiveService
            {
                Id = Guid.NewGuid(),
                Name = "minio",
                PaymentId = payment.Id,
                Price = 1611
            }
        ];
    }
}