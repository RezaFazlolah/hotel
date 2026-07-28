using Api;
using Api.MiddleWares;
using Application;
using Domain;
using Infrastructure;
using Scalar.AspNetCore;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Scalar
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());

    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.ValidateMapperConfiguration();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();