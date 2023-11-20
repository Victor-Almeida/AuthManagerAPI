using AuthManager.Application;
using AuthManager.Domain;
using AuthManager.Infrastructure;
using AuthManager.Persistence;
using AuthManager.WebAPI.Setup;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddAuthentication()
    .AddJwtBearer();

builder
    .Services
    .AddAuthorization()
    .AddCarter()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder
    .Services
    .ConfigureOptions<AuthenticationOptionsSetup>()
    .ConfigureOptions<AuthOptionsSetup>()
    .ConfigureOptions<JwtBearerOptionsSetup>()
    .ConfigureOptions<SwaggerGenOptionsSetup>();

builder
    .Services
    .ConfigureDomain()
    .ConfigureInfrastructure()
    .ConfigurePersistence()
    .ConfigureApplication();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

app
    .UseSwagger()
    .UseSwaggerUI()
    .UseAuthentication()
    .UseAuthorization();

app.MapCarter();

app.Run();