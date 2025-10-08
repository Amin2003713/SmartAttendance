using System.Linq;
using System.Reflection;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Application.Features.Users.Commands.UpdateUser;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;
using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.Utilities.TypeConverters;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Tenants;
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
        OtherAdaptor();
        UserAdaptor();
        PlanAdaptor();
    }

    private static void PlanAdaptor()
    {
        TypeAdapterConfig<Plan, GetPlanInfoResponse>.NewConfig()
            .Map(dest => dest.Major,
                src => src.Major != null
                    ? new GetMajorInfoResponse()
                    {
                        Id = src.Major.Id,
                        Name = src.Major.Name,
                    }
                    : null)
            .Map(dest => dest.Attendances,
                src => src.Attendances.Any()
                    ? src.Attendances.Select(a =>
                        new GetMajorInfoResponse()
                        {
                            Id = a.Subject.Id,
                            Name = a.Subject.Name,
                        }
                    )
                    : null)
            .Map(dest => dest.Subjects,
                src => src.Subjects.Any()
                    ? src.Subjects.Select(a =>
                        new GetSubjectInfoResponse()
                        {
                            Id = a.Subject.Id,
                            Name = a.Subject.Name,
                        }
                    )
                    : null)
            ;
    }

    private static void UserAdaptor()
    {
        TypeAdapterConfig<UpdateUserCommand, User>.NewConfig()
            .Map(dest => dest.ProfilePicture,
                src => src.ProfilePicture != null && src.ProfilePicture.MediaUrl != null
                    ? src.ProfilePicture.MediaUrl
                    : null);

        TypeAdapterConfig<User, GetUserByIdResponse>.NewConfig()
            .Map(dest => dest.Profile,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(false)
                    : null)
            .Map(dest => dest.FullName,
                src => src.FullName())
            .Map(dest => dest.ProfileCompress,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(true)
                    : null);

        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(dest => dest.ProfilePicture,
                src => src.ProfilePicture != null
                    ? src.ProfilePicture!.BuildImageUrl(false)
                    : null);
    }

    private static void OtherAdaptor()
    {
        // Initial university creation
        TypeAdapterConfig<InitialUniversityCommand, UniversityTenantInfo>.NewConfig()
            .Map(dest => dest.Identifier, src => src.Domain);

        // University info response mapping
        TypeAdapterConfig<UniversityTenantInfo, GetUniversityInfoResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.Identifier)
            .Map(dest => dest.Logo,
                src => src.Logo != null
                    ? src.Logo!.BuildImageUrl(false)
                    : null);

        // User log info
        TypeAdapterConfig<User, GetUserResponse>.NewConfig()
            .Map(dest => dest.CreatedBy,
                src => src.CreatedBy == null
                    ? null
                    : new LogPropertyInfoResponse
                    {
                        Id = src.CreatedBy.Value
                    });

        // Tenant user mapping
        TypeAdapterConfig<UniversityUser, GetUserTenantResponse>.NewConfig()
            .Map(dest => dest.Domain, src => src.UniversityTenantInfo.Identifier)
            .Map(dest => dest.Name,   src => src.UniversityTenantInfo.Name);

        // Ignore IFormFile globally
        TypeAdapterConfig.GlobalSettings.Default.IgnoreMember((member, side) =>
            member.Type == typeof(IFormFile));
    }
}