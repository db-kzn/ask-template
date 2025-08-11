namespace ASK.Shared.Dto;

public record LoginRequest(
    string Email,
    string Password,
    string TenantId,
    string IpAddress);

public record RefreshTokenRequest(
    string RefreshToken,
    string IpAddress);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresOn);
