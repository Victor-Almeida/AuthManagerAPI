using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuthManager.WebAPI.Setup
{
    public sealed class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
    {
        public AuthenticationOptionsSetup() { }

        public void Configure(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
