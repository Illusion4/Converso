using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class JwtOptions : IJwtOptions
{
    public required string JwtIssuer { get; init; }
    public required string JwtAudience { get; init; }
    public required string JwtSecretKey { get; init; }
    public required int JwtExpiryInMinutes { get; init; }
    public required int RefreshTokenExpiryInDays { get; init; }
}