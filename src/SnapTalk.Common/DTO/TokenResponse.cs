namespace SnapTalk.Common.DTO;

public record TokenResponse(string AccessToken, Guid RefreshToken);