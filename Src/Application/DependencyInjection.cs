using Application.Behaviors;
using Application.Services;
using Domain.Interface;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<IAdminService, AdminService>();

        // Fluent Validation
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

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
}