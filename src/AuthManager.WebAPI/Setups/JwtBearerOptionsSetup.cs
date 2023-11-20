using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthManager.Infrastructure.Auth;
using System.Text;

namespace AuthManager.WebAPI.Setup
{
    public sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthOptions _authOptions;

        public JwtBearerOptionsSetup(IOptions<AuthOptions> authOptions) 
        {
            _authOptions = authOptions.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Audience = _authOptions.Audience;
            options.Authority = _authOptions.Issuer;
            options.RequireHttpsMetadata = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey)),
                RequireSignedTokens = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidAudience = _authOptions.Audience,
                ValidIssuer = _authOptions.Issuer
            };
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }
    }
}
