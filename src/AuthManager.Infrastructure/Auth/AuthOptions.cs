namespace AuthManager.Infrastructure.Auth;

public class AuthOptions
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
}