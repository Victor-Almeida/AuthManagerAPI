﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthManager.Infrastructure.Auth;
using System.Text;

namespace AuthManager.WebAPI.Setup
{
    public sealed class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
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
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey)),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                ValidAudience = _authOptions.Audience,
                ValidIssuer = _authOptions.Issuer,
            };
        }
    }
}
