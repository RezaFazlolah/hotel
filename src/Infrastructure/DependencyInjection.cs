using Application.Interfaces;
using Application.Interfaces.QueryServices;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Infrastructure.QueryServices;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
                    .UseSnakeCaseNamingConvention()
            );

            // identity
            services.AddIdentityCore<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>();

            // redis
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = configuration.GetConnectionString("Redis"));

            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .ValidateOnStart();
            services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<IPaginator, Paginator>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHotelQueryService, HotelQueryService>();

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomQueryService, RoomQueryService>();

            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReservationQueryService,
                ReservationQueryService>();

            return services;
        }
    }
}