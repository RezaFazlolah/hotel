using System.Text;
using Api.Services;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token here"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        // HttpContextAccessor
        services.AddHttpContextAccessor();

        // AutoMapper
        services.AddAutoMapper(_ => { }, typeof(ApiAssemblyMarker).Assembly);

        services.AddControllers();

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("JwtSettings"))
            .ValidateOnStart();

        // auth
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtSettings>>((options, jwtOptions) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Value.Issuer,
                    ValidAudience = jwtOptions.Value.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key))
                };
                options.MapInboundClaims = false;
            });

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}