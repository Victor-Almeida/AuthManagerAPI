namespace AuthManager.Application.ViewModels;

public record AuthViewModel(
    string BearerToken,
    Guid UserId,
    string Username);
