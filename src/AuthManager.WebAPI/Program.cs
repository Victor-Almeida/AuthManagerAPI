using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using AuthManager.Application;
using AuthManager.Domain;
using AuthManager.Infrastructure;
using AuthManager.Persistence;
using AuthManager.WebAPI.Setup;
using Carter;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<AuthOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder
    .Services
    .AddAuthorization(options => 
    {
        options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
    })
    .AddCarter()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Auth Manager API",
            Version = "v1"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Description = "Please enter a valid token",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Scheme = "Bearer",
            Type = SecuritySchemeType.ApiKey
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });

builder.Services
    .ConfigureDomain()
    .ConfigureInfrastructure()
    .ConfigurePersistence()
    .ConfigureApplication();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app
    .UseSwagger()
    .UseSwaggerUI()
    .UseAuthentication()
    .UseAuthorization();

app.MapCarter();

app.Run();