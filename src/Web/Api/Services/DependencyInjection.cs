using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
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
using Shifty.Api.Filters;
using Shifty.ApiFramework.Aspire;
using Shifty.ApiFramework.Attributes;
using Shifty.ApiFramework.Middleware.Tenant;
using Shifty.ApiFramework.Swagger;
using Shifty.Application.Users.Requests.Login;
using Shifty.Common;
using Shifty.Common.Behaviours;
using Shifty.Common.General;
using Shifty.Common.Utilities;
using Shifty.Domain.Constants;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.MigrationManagers;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shifty.Api.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services , IConfiguration configuration , SiteSettings siteSettings)
        {
            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
            var appOptions             = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            var distributedCacheConfig = configuration.GetSection(nameof(DistributedCacheConfig)).Get<DistributedCacheConfig>();


            services.AddScoped<IAppOptions , AppOptions>(_ => appOptions);

            services.AddSingleton<IMultiTenantContext<ShiftyTenantInfo> , MultiTenantContext<ShiftyTenantInfo>>();


            services.AddHttpContextAccessor();
            services.AddCustomIdentity();
            services.AddJwtAuthentication(siteSettings.JwtSettings);
            services.AddServiceControllers();
            services.AddPolyCache(configuration);

            // services.AddHealthChecks()
            //         .AddSqlServer(appOptions.WriteDatabaseConnectionString)
            //         .AddRedis(distributedCacheConfig.ConnectionString);
            // services.AddHealthChecksUI()
            //         .AddInMemoryStorage();

            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(UnhandledExceptionBehaviour<,>));

            // Register the MigrationService
            services.AddScoped<IMigrationService , MigrationService>();
            services.AddRouting(a=>
                                {
                                    a.LowercaseUrls = true;
                                    a.AppendTrailingSlash = true;
                                    a.LowercaseQueryStrings = true;
                                });

            services.AddMultiTenant<ShiftyTenantInfo>().
                     WithHostStrategy("__tenant__.*").
                     WithHeaderStrategy().
                     WithEFCoreStore<TenantDbContext , ShiftyTenantInfo>();

            services.AddAspire();

            services.AddSwaggerOptions();


            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app , IConfiguration configuration , IWebHostEnvironment env)
        {
            app.UseCors(builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        });

            app.UseMultiTenant();
            app.UseMiddleware<TenantValidationMiddleware>(); // Add this before your endpoints


            app.UseAppSwagger(configuration);
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 if (env.IsDevelopment() || env.IsStaging())
                                 {
                                     endpoints.MapControllers().AllowAnonymous();
                                 }
                                 else
                                 {
                                     endpoints.MapControllers();
                                 }

                                 // endpoints.MapHealthChecksUI();
                                 // endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                                 // {
                                 //     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                 // });
                             });


            return app;
        }

        private static void AddJwtAuthentication(this IServiceCollection services , JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
                                       }).
                     AddJwtBearer(options =>
                                  {
                                      var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

                                      var validationParameters = new TokenValidationParameters
                                      {
                                          ClockSkew                  = TimeSpan.Zero , // default: 5 min
                                          RequireSignedTokens        = true
                                          , ValidateIssuerSigningKey = true , IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                                          , RequireExpirationTime    = true
                                          , ValidateLifetime         = true , ValidateAudience = true , //default : false
                                          ValidAudience              = jwtSettings.Audience
                                          , ValidateIssuer           = true , //default : false
                                          ValidIssuer                = jwtSettings.Issuer
                                          ,
                                      };

                                      options.RequireHttpsMetadata      = false;
                                      options.SaveToken                 = true;
                                      options.TokenValidationParameters = validationParameters;

                                      options.Events = new JwtBearerEvents
                                      {
                                          OnAuthenticationFailed = context =>
                                                                   {
                                                                       if (context.Exception != null)
                                                                           throw new ShiftyException(ApiResultStatusCode.UnAuthorized
                                                                               , "Authentication failed."
                                                                               , HttpStatusCode.Unauthorized
                                                                               , context.Exception
                                                                               , null);

                                                                       return Task.CompletedTask;
                                                                   }
                                          , OnTokenValidated = async context => await AddLoginRecordForUsers(context) , OnChallenge = context =>
                                          {
                                              if (context.AuthenticateFailure != null)
                                                  throw new ShiftyException(ApiResultStatusCode.UnAuthorized
                                                      , "Authenticate failure."
                                                      , HttpStatusCode.Unauthorized
                                                      , context.AuthenticateFailure
                                                      , null);
                                              throw new ShiftyException(ApiResultStatusCode.UnAuthorized
                                                  , "You are unauthorized to access this resource."
                                                  , HttpStatusCode.Unauthorized);
                                          }
                                          ,
                                      };
                                  });
        }

        private static async Task AddLoginRecordForUsers(TokenValidatedContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims.Any() != true)
                context.Fail("This token has no claims.");

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

            if (!user.IsActive)
                context.Fail("User is not active.");

            await userRepository.UpdateLastLoginDateAsync(user , context.HttpContext.RequestAborted);
        }

        private static void AddServiceControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                                    {
                                        options.Filters.Add(typeof(ValidateModelStateAttribute));
                                        options.Filters.Add(new ApiExceptionFilter());
                                    });

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

                                                  identityOptions.SignIn.RequireConfirmedPhoneNumber = true;
                                                  identityOptions.Lockout.MaxFailedAccessAttempts    = 10;
                                                  identityOptions.Lockout.DefaultLockoutTimeSpan     = TimeSpan.FromMinutes(10);
                                                  identityOptions.Lockout.AllowedForNewUsers         = true;
                                              }).
                     AddEntityFrameworkStores<WriteOnlyDbContext>().
                     AddTokenProvider<PhoneNumberTokenProvider<User>>(ApplicationConstant.CodeGenerator).
                     AddDefaultTokenProviders();
        }

        #region Swagger

        private static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();
            // Add FV Rules to swagger
            services.AddFluentValidationRulesToSwagger();
            services.AddSwaggerGen(options =>
                                   {

                                       options.EnableAnnotations();

                                       options.SwaggerDoc("v1"
                                           , new OpenApiInfo
                                           { 
                                               Title = "Shifty.Apis"
                                               , Contact = new OpenApiContact
                                               {
                                                   Name  = "Amin Ahmadi" , Email = "amin1382amin@gmail.com"
                                                   , Url = new Uri("https://github.com/Amin2003713") ,
                                               }
                                               ,
                                           });

                                       #region Filters

                                       // Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                                        options.ExampleFilters();

                                       options.OperationFilter<ApplySummariesOperationFilter>();
                                       options.OperationFilter<ApplyHeaderParameterOperationFilter>();

                                       //Add 401 response and security requirements (Lock icon) to actions that need authorization
                                       options.OperationFilter<UnauthorizedResponsesOperationFilter>(true , "OAuth2");

                                       options.AddSecurityDefinition("Bearer"
                                           , new OpenApiSecurityScheme
                                           {
                                               Name = "Authorization" , Type = SecuritySchemeType.ApiKey , Scheme = "Bearer" , BearerFormat = "JWT"
                                               , In = ParameterLocation.Header , Description = "JWT Authorization header using the Bearer scheme." ,
                                           });
                                       

                                       options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                       {
                                           {
                                               new OpenApiSecurityScheme
                                               {
                                                   Reference = new OpenApiReference
                                                   {
                                                       Type = ReferenceType.SecurityScheme , Id = "Bearer" ,
                                                   }
                                                   ,
                                               }
                                               , Array.Empty<string>()
                                           } ,

                                       });

                                       #endregion
                                   });

            return services;
        }


        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app , IConfiguration configuration)
        {
            app.UseSwagger();

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
                             {
                                 options.SwaggerEndpoint("/swagger/v1/swagger.json" , "Shifty.WebUI v1");

                                 options.DocExpansion(DocExpansion.None);
                             });

            //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
            app.UseReDoc(options =>
                         {
                             options.SpecUrl("/swagger/v1/swagger.json");

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
                             options.DisableSearch();
                             options.OnlyRequiredInSamples();
                             options.SortPropsAlphabetically();

                             #endregion
                         });
            return app;
        }

        #endregion
    }
}