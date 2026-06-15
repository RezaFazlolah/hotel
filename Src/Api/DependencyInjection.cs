using System.Text;
using Api.Services;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServicess(this IServiceCollection services, IConfiguration configuration)
    {
        // scalar
        services.AddOpenApi();

        // swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // HttpContextAccessor
        services.AddHttpContextAccessor();

        // AutoMapper
        services.AddAutoMapper(_ => { }, typeof(ApiAssemblyMarker).Assembly);

        services.AddControllers();

        // auth
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                    options.MapInboundClaims = false;
                }
            );

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}