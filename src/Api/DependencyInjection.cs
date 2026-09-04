using System.Text;
using System.Text.Json.Serialization;
using Api.ExceptionHandlers;
using Api.Services;
using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiServices(string applicationName)
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

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
            });

            // AutoMapper
            services.AddAutoMapper(_ => { }, typeof(ApiAssemblyMarker).Assembly);

            // OpenTelemetry
            services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                    resource.AddService(serviceName: applicationName))
                .WithTracing(tracing =>
                    tracing.AddAspNetCoreInstrumentation()
                        .AddConsoleExporter());

            services.AddHttpContextAccessor();

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();

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
}