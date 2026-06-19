using System.Text;
using Api.Services;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Configuration;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
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

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ??
                          throw new InvalidOperationException("JwtSettings is null.");
        
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
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                    options.MapInboundClaims = false;
                }
            );
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}