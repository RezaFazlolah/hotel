using Application.Common.Behaviors;
using Application.Common.Paginations;
using Application.Services;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices(
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
            
            return services;
        }
    }
}