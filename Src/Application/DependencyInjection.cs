using Application.Common.Behaviors;
using Application.Configurations;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Interfaces;
using SharedKernel.Paginations;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IManagerService, ManagerService>();

        services.AddScoped<IReservationService, ReservationService>();

        // Fluent Validation
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // FluentValidation
        });

        // AutoMapper
        services.AddAutoMapper(_ => { }, typeof(ApplicationAssemblyMarker).Assembly);

        services.AddOptions<PaginationSettings>()
            .Bind(configuration.GetSection(PaginationSettings.SectionName))
            .Validate(ps => ps.MaxPageSize > 0, "PaginationSettings:MaxPageSize must be greater than 0")
            .ValidateOnStart();
        
        services.AddSingleton<IPaginator, Paginator>();

        return services;
    }

    // Application/DependencyInjection.cs/ValidateMapperConfiguration()
    public static void ValidateMapperConfiguration(this IServiceProvider serviceProvider)
    {
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}