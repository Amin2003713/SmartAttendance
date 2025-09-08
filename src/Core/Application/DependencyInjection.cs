using System.Reflection;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Application.Base.Companies.Commands.InitialCompany;
using SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;
using SmartAttendance.Application.Base.Discounts.Commands.CreateDiscount;
using SmartAttendance.Application.Base.Payment.Request.Queries.ListPayment;
using SmartAttendance.Application.Features.Users.Commands.UpdateUser;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;
using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.Utilities.TypeConverters;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Domain.Tenants.Discounts;
using SmartAttendance.Domain.Tenants.Payments;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddCustomFluentValidation();
        ConfigureMaster();
        return services;
    }

    private static void AddCustomFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static void ConfigureMaster()
    {
        DiscountAdaptor();
        OtherAdaptor();
        UserAdaptor();
    }


    private static void UserAdaptor()
    {
        TypeAdapterConfig<UpdateUserCommand, User>.NewConfig()
            .Map(dest => dest.Profile,
                src => src.ImageFile != null && src.ImageFile.MediaUrl != null
                    ? src.ImageFile.MediaUrl
                    : null);

        TypeAdapterConfig<User, GetUserByIdResponse>.NewConfig()
            .Map(
                dest => dest.Profile,
                src => src.Profile != null
                    ? src.Profile!.BuildImageUrl(false)
                    : null)
            .Map(dest => dest.ProfileCompress,
                src => src.Profile != null
                    ? src.Profile!.BuildImageUrl(true)
                    : null);

        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(
                dest => dest.Profile,
                src => src.Profile != null
                    ? src.Profile!.BuildImageUrl(false)
                    : null)
            .Map(dest => dest.ProfileCompress,
                src => src.Profile != null
                    ? src.Profile!.BuildImageUrl(true)
                    : null);
    }

    private static void OtherAdaptor()
    {
        TypeAdapterConfig<InitialCompanyCommand, SmartAttendanceTenantInfo>.NewConfig()
            .Map(dest => dest.Identifier, src => src.Domain);


        TypeAdapterConfig<SmartAttendanceTenantInfo, GetCompanyInfoResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.Identifier)
            .Map(
                dest => dest.Logo,
                src => src.Logo != null
                    ? src.Logo!.BuildImageUrl(false)
                    : null);


        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(dest => dest.CreatedBy,
                src => src.CreatedBy == null
                    ? null
                    : new LogPropertyInfoResponse
                    {
                        Id = src.CreatedBy.Value
                    });


        TypeAdapterConfig<TenantUser, GetUserTenantResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.SmartAttendanceTenantInfo.Identifier)
            .Map(dest => dest.Name,   src => src.SmartAttendanceTenantInfo.Name);


        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) => member.Type == typeof(IFormFile));
    }


    private static void DiscountAdaptor()
    {
        TypeAdapterConfig<CreateDiscountCommand, Discount>.NewConfig().Map(dest => dest.EndDate, src => src.StartDate.AddDays(src.Duration));

        TypeAdapterConfig<Payments, PaymentQueryResponse>.NewConfig()
            .Map(dest => dest.Status, src => src.Status == 100 || src.Status == 101);
        // .Map(dest => dest.AcvtiveServices,
        //     src => src.ActiveServices.Any()
        //         ? src.ActiveServices.Select(a => a.Name)
        //         : new List<string>().AsReadOnly());
    }
}