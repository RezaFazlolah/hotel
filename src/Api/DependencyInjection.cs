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

namespace Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiServices()
        {
            services.AddOpenApi();

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

            services.AddHttpContextAccessor();

            services.AddAutoMapper(_ => { }, typeof(ApiAssemblyMarker).Assembly);

            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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
            
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}