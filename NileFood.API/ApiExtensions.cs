using Microsoft.OpenApi.Models;

namespace NileFood.API;

public static class ApiExtensions
{
    public static IServiceCollection AddApiExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCorsConfig(configuration)
            .AddControllers();

        services.AddSwaggerConfiguration();

        //var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates");
        //services.AddSingleton<ITemplateReader>(new FileTemplateReader(templatePath));

        return services;
    }


    private static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigin = configuration.GetSection("AllowedOrigin").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .WithExposedHeaders("Content-Disposition");
            });
        });


        return services;
    }


    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Restaurant API",
                Version = "v1",
                Description = "API Documentation for NileFood with JWT Authentication"
            });

            // 🔐 إعداد زر Authorize لاستخدام JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
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
        });

        return services;
    }


}
