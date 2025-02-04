using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PolyCache;
using Scalar.AspNetCore;
using Serilog.Enrichers.Correlate;
using Shifty.Api.Filters;
using Shifty.ApiFramework.Attributes;
using Shifty.ApiFramework.Middleware.Tenant;
using Shifty.ApiFramework.Swagger;
using Shifty.Application.Users.Requests.Login;
using Shifty.Common;
using Shifty.Common.Behaviours;
using Shifty.Common.General;
using Shifty.Common.Utilities;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Resources.Messages;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Shifty.ApiFramework.Analytics;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Users;
using Shifty.Domain.Interfaces.Features.Users;

namespace Shifty.Api.Services
{
    public static class DependencyInjection
    {
        public static void AddWebApi(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddSingleton<IMultiTenantContext<ShiftyTenantInfo> , MultiTenantContext<ShiftyTenantInfo>>();
            services.AddCorrelationContextEnricher();
            services.AddSwaggerOptions();
            services.AddHttpContextAccessor();
            services.AddCustomIdentity();
            services.AddJwtAuthentication();
            services.AddServiceControllers();
            services.AddPolyCache(configuration);
            services.AddEndpointsApiExplorer();

            services.AddObservabilityServices();
            services.AddTransient<CorrelationIdMiddleware>();
            // services.AddHealthShiftyCheck();

            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(RequestResponseLoggingBehavior<,>));

            // Register the MigrationService
            services.AddRouting(a =>
                                {
                                    a.LowercaseUrls         = true;
                                    a.AppendTrailingSlash   = true;
                                    a.LowercaseQueryStrings = true;
                                });

            services.AddMultiTenant<ShiftyTenantInfo>().
                     WithHostStrategy("__tenant__.*").
                     WithHeaderStrategy("--tenant--").
                     WithEFCoreStore<TenantDbContext , ShiftyTenantInfo>();

            services.AddScoped<ApiExceptionFilter>();
            services.AddScoped<ValidateModelStateAttribute>();
        }

        // private static void AddHealthShiftyCheck(this IServiceCollection services)
        // {
        //     services.AddHealthChecks().AddSqlServer(ApplicationConstant.AppOptions.TenantStore).AddCheck<TenantDatabaseHealthCheck>("tenant Dbs");
        //     services.AddHealthChecksUI(options =>
        //                                {
        //                                    options.SetEvaluationTimeInSeconds(30); // Configures the UI to refresh health status every 60 seconds
        //                                    options.SetMinimumSecondsBetweenFailureNotifications(60); // Sets minimum time between failure notifications
        //                                    options.MaximumHistoryEntriesPerEndpoint(500); // Configures how many history entries to keep
        //                                }).
        //              AddSqlServerStorage(ApplicationConstant.AppOptions.TenantStore);
        // }

        public static void UseWebApi(this IApplicationBuilder app , IWebHostEnvironment env)
        {
            app.UseCors(builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        });

            app.UseMultiTenant();
            app.UseMiddleware<TenantValidationMiddleware>(); // Add this before your endpoints
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseSerilogRequestLogging();

            app.UseAppSwagger();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 if (env.IsDevelopment())
                                 {
                                     endpoints.MapControllers().AllowAnonymous();
                                 }
                                 else
                                 {
                                     endpoints.MapControllers();
                                 }

                                 endpoints.MapScalarApiReference(opt =>
                                                                 {
                                                                     opt.OpenApiRoutePattern = "/api/openapi/{documentName}.json";
                                                                     // Set the title for the API Reference
                                                                     opt.Title = "Shifty API Reference";
                                                                     // Customize the theme (use predefined themes or provide a custom one)
                                                                     opt.Theme  = ScalarTheme.BluePlanet; // Available themes: Light, Dark, Custom
                                                                     opt.Layout = ScalarLayout.Classic;
                                                                     // Configure default HTTP client
                                                                     opt.DefaultHttpClient =
                                                                         new KeyValuePair<ScalarTarget , ScalarClient>(ScalarTarget.Http , ScalarClient.Axios);

                                                                     opt.DarkMode = true;
                                                                     opt.EnabledClients = new[]
                                                                     {
                                                                         ScalarClient.Axios ,
                                                                         ScalarClient.Http2 ,
                                                                         ScalarClient.Httpie ,
                                                                         ScalarClient.Requests ,
                                                                         ScalarClient.Native ,
                                                                         ScalarClient.HttpClient ,
                                                                         ScalarClient.Curl ,
                                                                     };

                                                                     opt.OperationSorter = OperationSorter.Method;
                                                                 });

                                  // endpoints.MapHealthChecksUI();
                                  // endpoints.MapHealthChecks("/api/health", new HealthCheckOptions()
                                  // {
                                  //     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                                  // });
                             });



            // app.UseHealthChecksUI(options => options.UIPath = "/api/health-ui");
        }

        private static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
                                       }).
                     AddJwtBearer(options =>
                                  {
                                      var secretKey = Encoding.UTF8.GetBytes(ApplicationConstant.JwtSettings.SecretKey);

                                      var validationParameters = new TokenValidationParameters
                                      {
                                          ClockSkew             = TimeSpan.Zero , // default: 5 min
                                          RequireSignedTokens   = true , ValidateIssuerSigningKey = true ,
                                          IssuerSigningKey      = new SymmetricSecurityKey(secretKey) ,
                                          RequireExpirationTime = true , ValidateLifetime               = true , ValidateAudience = true , //default : false
                                          ValidAudience         = ApplicationConstant.JwtSettings.Audience , ValidateIssuer = true ,                           //default : false
                                          ValidIssuer           = ApplicationConstant.JwtSettings.Issuer ,
                                      };

                                      options.RequireHttpsMetadata      = false;
                                      options.SaveToken                 = true;
                                      options.TokenValidationParameters = validationParameters;

                                      options.Events = new JwtBearerEvents
                                      {
                                          OnAuthenticationFailed = context =>
                                                                   {
                                                                       if (context.Exception != null)
                                                                           throw ShiftyException.Unauthorized(additionalData: context.Exception);

                                                                       return Task.CompletedTask;
                                                                   } ,
                                          OnTokenValidated = async context => await AddLoginRecordForUsers(context) , OnChallenge = context =>
                                          {
                                              if (context.AuthenticateFailure != null)
                                                  throw ShiftyException.Unauthorized(additionalData: context.AuthenticateFailure);
                                              return Task.CompletedTask;
                                          } ,
                                      };
                                  });
        }

        private static async Task AddLoginRecordForUsers(TokenValidatedContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var userMessage    = context.HttpContext.RequestServices.GetRequiredService<UserMessages>();

            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims.Any() != true)
                throw ShiftyException.Forbidden(userMessage.InValid_Token());


            //var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
            //if (!securityStamp.HasValue())
            //    context.Fail("This token has no security stamp");

            //Find user and token from database and perform your custom validation
            var userId = Guid.Parse(claimsIdentity.GetUserId<string>());
            var user   = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted , userId);

            //if (user.SecurityStamp != Guid.Parse(securityStamp))
            //    context.Fail("Token security stamp is not valid.");

            //var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
            //if (validatedUser == null)
            //    context.Fail("Token security stamp is not valid.");

            if (!user.IsActive || !user.PhoneNumberConfirmed)
                throw ShiftyException.Forbidden(userMessage.User_Error_NotActive());

            await userRepository.UpdateLastLoginDateAsync(user , context.HttpContext.RequestAborted);
        }

        private static void AddServiceControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                                    {
                                        options.Filters.Add<ValidateModelStateAttribute>();
                                        options.Filters.Add<ApiExceptionFilter>();
                                    }).
                     AddDataAnnotationsLocalization().
                     AddMvcLocalization().
                     AddViewLocalization();

            services.AddCors();
        }

        private static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User , Role>(identityOptions =>
                                              {
                                                  //Password Settings
                                                  identityOptions.Password.RequireDigit           = true;
                                                  identityOptions.Password.RequiredLength         = 8;
                                                  identityOptions.Password.RequireNonAlphanumeric = false;
                                                  identityOptions.Password.RequireUppercase       = false;
                                                  identityOptions.Password.RequireLowercase       = false;

                                                  //UserName Settings
                                                  identityOptions.User.RequireUniqueEmail = false;

                                                  identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                                                  identityOptions.Lockout.MaxFailedAccessAttempts    = 10;
                                                  identityOptions.Lockout.DefaultLockoutTimeSpan     = TimeSpan.FromMinutes(10);
                                                  identityOptions.Lockout.AllowedForNewUsers         = true;
                                              }).
                     AddEntityFrameworkStores<WriteOnlyDbContext>().
                     AddTokenProvider<PhoneNumberTokenProvider<User>>(ApplicationConstant.Identity.CodeGenerator).
                     AddDefaultTokenProviders();
        }

        #region Swagger

        private static void AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();
            services.AddSwaggerGen(options =>
                                   {
                                       options.EnableAnnotations();
                                                         
                                       options.SwaggerDoc("v1" ,
                                           new OpenApiInfo
                                           {
                                               Title = "Shifty.Apis" , Contact = new OpenApiContact
                                               {
                                                   Name = "Amin Ahmadi" , Email = "amin1382amin@gmail.com" , Url = new Uri("https://github.com/Amin2003713") ,
                                               } ,
                                           });


                                       #region Filters

                                       options.ExampleFilters();

                                       options.OperationFilter<ApplySummariesOperationFilter>();
                                       //Add 401 response and security requirements (Lock icon) to actions that need authorization
                                       options.OperationFilter<UnauthorizedResponsesOperationFilter>(true , "OAuth2");

                                       options.AddSecurityDefinition("Bearer" ,
                                           new OpenApiSecurityScheme
                                           {
                                               Name = "Authorization" , Type = SecuritySchemeType.ApiKey , Scheme = "Bearer" , BearerFormat = "JWT" ,
                                               In   = ParameterLocation.Header , Description = "JWT Authorization header using the Bearer scheme." ,
                                           });


                                       options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                       {
                                           {
                                               new OpenApiSecurityScheme
                                               {
                                                   Reference = new OpenApiReference
                                                   {
                                                       Type = ReferenceType.SecurityScheme , Id = "Bearer" ,
                                                   } ,
                                               } ,
                                               Array.Empty<string>()
                                           } ,
                                       });
                                       options.OperationFilter<ApplyHeaderParameterOperationFilter>();

                                       #endregion

                                       #region Api_Docs
                                       // Get XML file path
                                       var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                                       var xmlPath = Path.Combine(AppContext.BaseDirectory , xmlFile);
                                       // Include XML comments
                                       options.IncludeXmlComments(xmlPath);
                                       #endregion
                                   });
        }


        private static void UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
                           {
                               options.RouteTemplate = "/api/openapi/{documentName}.json";
                           });

            // //Swagger middleware for generate UI from swagger.json
            // app.UseSwaggerUI(options =>
            //                  {
            //                      options.SwaggerEndpoint("/swagger/v1/swagger.json" , "Shifty.WebUI v1");
            //
            //                      options.DocExpansion(DocExpansion.None);
            //                  });
            //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
            app.UseReDoc(options =>
                         {
                             // options.SpecUrl("/swagger/v1/swagger.json");

                             #region Customizing

                             //By default, the ReDoc UI will be exposed at "/api-docs"
                             //options.RoutePrefix = "docs";
                             //options.DocumentTitle = "My API Docs";

                             options.EnableUntrustedSpec();
                             options.ScrollYOffset(10);
                             options.HideHostname();
                             options.HideDownloadButton();
                             options.ExpandResponses("200,201");
                             options.RequiredPropsFirst();
                             options.NoAutoAuth();
                             options.PathInMiddlePanel();
                             options.HideLoading();
                             options.NativeScrollbars();
                             options.OnlyRequiredInSamples();
                             options.SortPropsAlphabetically();

                             #endregion
                         });
        }

        #endregion


    }
}