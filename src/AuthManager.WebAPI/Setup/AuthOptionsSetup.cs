using Microsoft.Extensions.Options;
using AuthManager.Infrastructure.Auth;

namespace AuthManager.WebAPI.Setup
{
    public sealed class AuthOptionsSetup : IConfigureOptions<AuthOptions>
    {
        private const string SectionName = "JwtSettings";

        private readonly IConfiguration _configuration;

        public AuthOptionsSetup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void Configure(AuthOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
