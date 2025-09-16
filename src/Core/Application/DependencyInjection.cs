using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Application.Base.Companies.Commands.InitialCompany;
using SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;
using SmartAttendance.Application.Features.Users.Commands.UpdateUser;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;
using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.Utilities.TypeConverters;

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
        OtherAdaptor();
        UserAdaptor();
    }


    private static void UserAdaptor()
    {
        TypeAdapterConfig<UpdateUserCommand, User>.NewConfig().
                                                   Map(dest => dest.Profile,
                                                       src => src.ImageFile != null && src.ImageFile.MediaUrl != null
                                                           ? src.ImageFile.MediaUrl
                                                           : null);

        TypeAdapterConfig<User, GetUserByIdResponse>.NewConfig().
                                                     Map(
                                                         dest => dest.Profile,
                                                         src => src.Profile != null
                                                             ? src.Profile!.BuildImageUrl(false)
                                                             : null).
                                                     Map(dest => dest.ProfileCompress,
                                                         src => src.Profile != null
                                                             ? src.Profile!.BuildImageUrl(true)
                                                             : null);

        TypeAdapterConfig<User, GetUserResponse>.NewConfig().
                                                 Map(
                                                     dest => dest.Profile,
                                                     src => src.Profile != null
                                                         ? src.Profile!.BuildImageUrl(false)
                                                         : null).
                                                 Map(dest => dest.ProfileCompress,
                                                     src => src.Profile != null
                                                         ? src.Profile!.BuildImageUrl(true)
                                                         : null);
    }

    private static void OtherAdaptor()
    {
        TypeAdapterConfig<InitialCompanyCommand, SmartAttendanceTenantInfo>.NewConfig().Map(dest => dest.Identifier, src => src.Domain);


        TypeAdapterConfig<SmartAttendanceTenantInfo, GetCompanyInfoResponse>.NewConfig().
                                                                             Map(dest => dest.Domain, src => src.Identifier).
                                                                             Map(
                                                                                 dest => dest.Logo,
                                                                                 src => src.Logo != null
                                                                                     ? src.Logo!.BuildImageUrl(false)
                                                                                     : null);


        TypeAdapterConfig<User, GetUserResponse>.NewConfig().
                                                 Map(dest => dest.CreatedBy,
                                                     src => src.CreatedBy == null
                                                         ? null
                                                         : new LogPropertyInfoResponse
                                                         {
                                                             Id = src.CreatedBy.Value
                                                         });


        TypeAdapterConfig<TenantUser, GetUserTenantResponse>.NewConfig().
                                                             Map(dest => dest.Domain, src => src.SmartAttendanceTenantInfo.Identifier).
                                                             Map(dest => dest.Name,   src => src.SmartAttendanceTenantInfo.Name);


        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) => member.Type == typeof(IFormFile));
    }
}