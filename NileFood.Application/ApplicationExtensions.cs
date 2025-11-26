using FluentValidation.AspNetCore;
using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NileFood.Application.Authentication;
using NileFood.Application.Services.Implementations;
using NileFood.Application.Services.Interfaces;
using NileFood.Application.Settings;
using NileFood.Domain.Entities.Identity;
using NileFood.Infrastructure.Data;
using System.Reflection;

namespace NileFood.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddNileFoodServices()
            .AddAuthConfig(configuration)
            .AddMapsterServices()
            .AddFluentValidationConfig()
            .AddBackgroundJobsConfig(configuration)
            .AddEmailSenderConfiguration();



        return services;
    }

    private static IServiceCollection AddEmailSenderConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailService>();
        services.AddOptions<MailSettings>()
            .BindConfiguration(nameof(MailSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }


    private static IServiceCollection AddNileFoodServices(this IServiceCollection services)
    {
        // add scoped services

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IMenuItemService, MenuItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped(typeof(IFilterService<>), typeof(FilterService<>));
        services.AddScoped<IMenuItemOptionTypeService, MenuItemOptionTypeService>();

        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.AllowedUserNameCharacters = null;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key!)),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
            };

        });


        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }

    private static IServiceCollection AddMapsterServices(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));


        return services;
    }


    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }


    private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
            {
                SchemaName = "HangfireSchema"
            }));

        services.AddHangfireServer();

        return services;
    }
}