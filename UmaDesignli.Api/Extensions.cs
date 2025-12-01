using UmaDesignli.Api.Services;
using UmaDesignli.Api.Filters;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Application.Behaviors;
using UmaDesignli.Infrastructure.Persistence;
using UmaDesignli.Infrastructure.Persistence.Repositories;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Infrastructure.Token;
using MediatR;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace UmaDesignli.Api
{
    public static class Extensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<UmaDesignli.Application.Commands.Access.LoginCommand>();
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // Register FluentValidation validators
           // services.AddValidatorsFromAssembly(typeof(CreateEmployeeCommand).Assembly);

            // Register repositories
            // Register the concrete EmployeeRepository as singleton first
          //  services.AddSingleton<EmployeeRepository>();
            
            // Register interfaces pointing to the same singleton instance
           // services.AddSingleton<IEmployeeRepository>(sp => sp.GetRequiredService<EmployeeRepository>());
          //  services.AddSingleton<IRepository<Userapp>>(sp => sp.GetRequiredService<EmployeeRepository>());
            
            // Register generic repository for other entities
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IDataSeeder, DataSeeder>();
            services.AddHostedService<AppInitializer>();

            // Register TokenProvider
            services.AddSingleton<ITokenProvider, TokenProvider>();

            return services;
        }

        /// <summary>
        /// Add Swagger configuration
        /// </summary>
        /// <param name="services">services</param>
        /// <returns></returns>
        internal static IServiceCollection AddSwaggerGenConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new()
                {
                    Version = "v1",
                    Title = "Api-Uma-Designli",
                    Description = "API for Designli"
                });

                // Add XML comments from API
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    config.IncludeXmlComments(xmlPath);
                }

                // Add XML comments from Domain
                var domainXmlFile = "UmaDesignli.Domain.xml";
                var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
                if (File.Exists(domainXmlPath))
                {
                    config.IncludeXmlComments(domainXmlPath);
                }

                // Use custom enum filter to show descriptions
                config.SchemaFilter<EnumSchemaFilter>();

                // Configure JWT authentication in Swagger
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
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
}
