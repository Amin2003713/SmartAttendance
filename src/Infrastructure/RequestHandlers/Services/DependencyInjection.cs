using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SmartAttendance.RequestHandlers.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddHandler(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddMediatR(cfg =>
        {
            cfg.RegisterGenericHandlers = true;
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Weather , GetWeatherDailyReportResponse> ,
        //         List<GetWeatherDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Weather , GetWeatherDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Weather , GetWeatherDailyReportResponse> ,
        //         List<GetWeatherDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Weather , GetWeatherDailyReportResponse>>();


        //
        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Staff , GetStaffDailyReportResponse> , List<GetStaffDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Staff , GetStaffDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Staff , GetStaffDailyReportResponse> ,
        //         List<GetStaffDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Staff , GetStaffDailyReportResponse>>();

        //
        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Tool , GetToolDailyReportResponse> , List<GetToolDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Tool , GetToolDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Tool , GetToolDailyReportResponse> ,
        //         List<GetToolDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Tool , GetToolDailyReportResponse>>();


        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Session , GetSessionDailyReportResponse> ,
        //         List<GetSessionDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Session , GetSessionDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Session , GetSessionDailyReportResponse> ,
        //         List<GetSessionDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Session , GetSessionDailyReportResponse>>();

        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Problem , GetProblemDailyReportResponse> ,
        //         List<GetProblemDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Problem , GetProblemDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Problem , GetProblemDailyReportResponse> ,
        //         List<GetProblemDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Problem , GetProblemDailyReportResponse>>();

        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<Progress , GetProgressDailyReportResponse> ,
        //         List<GetProgressDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<Progress , GetProgressDailyReportResponse>>();
        //
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<Progress , GetProgressDailyReportResponse> ,
        //         List<GetProgressDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<Progress , GetProgressDailyReportResponse>>();


        // services.AddTransient<
        //     IRequestHandler<GetDailyReportQuery<WorkPermit , GetWorkPermitDailyReportResponse> ,
        //         List<GetWorkPermitDailyReportResponse>> ,
        //     GetDailyReportQueryHandler<WorkPermit , GetWorkPermitDailyReportResponse>>();
        // services.AddTransient<
        //     IRequestHandler<GetComprehensiveReportQuery<WorkPermit , GetWorkPermitDailyReportResponse> ,
        //         List<GetWorkPermitDailyReportResponse>> ,
        //     GetComprehensiveReportHandler<WorkPermit , GetWorkPermitDailyReportResponse>>();
        return services;
    }
}