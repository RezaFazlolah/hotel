using Application.Common.Behaviors;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
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

        return services;
    }

    // Application/DependencyInjection.cs/ValidateMapperConfiguration()
    public static void ValidateMapperConfiguration(this IServiceProvider serviceProvider)
    {
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}