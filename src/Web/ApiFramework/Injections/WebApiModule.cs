using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Riviera.ZarinPal.V4;
using Serilog.Enrichers.Correlate;
using Shifty.ApiFramework.Analytics;
using Shifty.ApiFramework.Attributes;
using Shifty.ApiFramework.Configuration;
using Shifty.ApiFramework.Filters;
using Shifty.ApiFramework.Middleware.Tenant;
using Shifty.ApiFramework.Swagger;
using Shifty.ApiFramework.Tools;
using Shifty.Common.Behaviours;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Common.Utilities.IdentityHelpers;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.Seeder;
using Shifty.Persistence.Services.Taskes;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.ApiFramework.Injections;

public static class WebApiModule
{
    public static void AddWebApi<TUser, TRole, TTenantInfo, TTenantDbContext, TServiceDb, TStartUp, TApplication>(
        this IServiceCollection services,
        Func<TokenValidatedContext, Task> AddLoginRecordForUsers,
        string resourcesPath = "Resources",
        string tenantStoreConnection = null!,
        string swaggerTitle = null!)
        where TUser : class
        where TRole : class
        where TStartUp : class
        where TTenantInfo : class, ITenantInfo, new()
        where TServiceDb : DbContext
        where TTenantDbContext : EFCoreStoreDbContext<TTenantInfo>
    {
        tenantStoreConnection ??= ApplicationConstant.AppOptions.TenantStore;
        swaggerTitle ??= $"Web Api service of {Assembly.GetCallingAssembly().GetName().Name}";


        services.AddSingleton<IMultiTenantContext<TTenantInfo>, MultiTenantContext<TTenantInfo>>();
        services.AddHttpContextAccessor();
        services.AddCorrelationContextEnricher();
        services.AddZarinPal();

        services.AddSwaggerGenWithOptions<TApplication>(swaggerTitle);

        services.AddCustomIdentity<TUser, TRole, TServiceDb>();
        services.AddJwtAuthentication<TStartUp>(AddLoginRecordForUsers);
        services.AddServiceControllers();

        services.AddEndpointsApiExplorer();
        services.AddHangFireConfiguration(tenantStoreConnection);

        services.AddHybridCaching();

        services.AddTransient<CorrelationIdMiddleware>();
        services.AddTransient<TenantValidationMiddleware>();
        services.AddTransient<HandelArchive>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestResponseLoggingBehavior<,>));

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddMultiTenant<TTenantInfo>()
            .WithHostStrategy("__tenant__.*")
            .WithHeaderStrategy("X-Tenant")
            .WithEFCoreStore<TTenantDbContext, TTenantInfo>();

        services.AddScoped<ValidateModelStateAttribute>();
        services.AddLocalization(options => options.ResourcesPath = resourcesPath);

        services.AddCustomHealthChecks();
    }


    public static void AddZarinPal(this IServiceCollection services)
    {
        services.Configure<ZarinPalOptions>(options =>
        {
            options.MerchantId = "61c24dc8-870b-4e1f-8da4-733127086510";
            options.IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        });

        services.AddHttpClient(); 
        services.AddHttpClient<ZarinPalService>();
    }


    private static void AddSwaggerGenWithOptions<TExample>(this IServiceCollection services, string title)
    {
        services.AddSwaggerExamplesFromAssemblyOf<TExample>();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = title,
                    Contact = new OpenApiContact
                    {
                        Name = "Amin Ahmadi",
                        Email = "amin1382amin@gmail.com",
                        Url = new Uri("https://github.com/Amin2003713")
                    }
                });


        #region Filters

            options.ExampleFilters();

            options.OperationFilter<ApplySummariesOperationFilter>();
            //Add 401 response and security requirements (Lock icon) to actions that need authorization
            options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");
            options.SchemaFilter<EnumSchemaFilter>();


            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });


            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.OperationFilter<ApplyHeaderParameterOperationFilter>();

        #endregion

        #region Api_Docs

            // Get XML file path
            var xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // Include XML comments
            options.IncludeXmlComments(xmlPath);

        #endregion
        });
    }

    private static void AddCustomIdentity<TUser, TRole, TDbContext>(this IServiceCollection services)
        where TUser : class
        where TRole : class
        where TDbContext : DbContext
    {
        services.AddIdentity<TUser, TRole>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = false;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = false;

                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<TDbContext>()
            .AddTokenProvider<PhoneNumberTokenProvider<TUser>>(ApplicationConstant.Identity.CodeGenerator)
            .AddDefaultTokenProviders();
    }

    private static void AddJwtAuthentication<TStartUp>(
        this IServiceCollection services,
        Func<TokenValidatedContext, Task> AddLoginRecordForUsers)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;


                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var localizer = context.HttpContext.RequestServices
                            .GetRequiredService<IStringLocalizer<TStartUp>>();

                        if (context.Exception != null)
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                                throw ShiftyException.Unauthorized(localizer["Token Expired."].Value);

                            if (context.Exception is UnauthorizedAccessException)
                                throw ShiftyException.Unauthorized(localizer["Unauthorized access."].Value);

                            if (context.Exception is ForbiddenException)
                                throw ShiftyException.Forbidden(context.Exception?.Message);

                            throw ShiftyException.Unauthorized(context.Exception?.Message ??
                                                            localizer["Unauthorized access."].Value);
                        }


                        return Task.CompletedTask;
                    },
                    OnTokenValidated = AddLoginRecordForUsers,
                    OnChallenge = context =>
                    {
                        var localizer = context.HttpContext.RequestServices
                            .GetRequiredService<IStringLocalizer<TStartUp>>();

                        if (context.AuthenticateFailure != null)
                            throw ShiftyException.Unauthorized(
                                context.AuthenticateFailure?.Message ??
                                localizer["Unauthorized access."].Value);


                        return Task.CompletedTask;
                    },
                    OnMessageReceived = async context =>
                    {
                        var tenantSecretKey =
                            context.HttpContext.GetMultiTenantContext<ShiftyTenantInfo>().TenantInfo !=
                            null
                                ? await context.HttpContext.GenerateShuffledKeyAsync()
                                : new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(ApplicationConstant.JwtSettings.SecretKey));

                        var tenantValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            RequireSignedTokens = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = tenantSecretKey,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidAudience = ApplicationConstant.JwtSettings.Audience,
                            ValidateIssuer = true,
                            ValidIssuer = ApplicationConstant.JwtSettings.Issuer
                        };

                        options.TokenValidationParameters = tenantValidationParameters;
                    }
                };
            });
    }


    private static void AddServiceControllers(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelStateAttribute>();
                options.Filters.Add<ApiExceptionFilter>();
                options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider());
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
            .AddDataAnnotationsLocalization()
            .AddMvcLocalization()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .AddViewLocalization();

        services.AddCors();
    }

    private static void AddHangFireConfiguration(this IServiceCollection services, string connection)
    {
        services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connection,
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = Environment.ProcessorCount * 5;

            options.Queues = new[]
            {
                "critical",
                "default"
            };
        });

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
        {
            Attempts = 3,
            DelaysInSeconds = [10, 30, 60]
        });

        services.AddTransient<SeedCalendarService>();
        services.AddScoped<ScheduledTaskService>();
    }
}