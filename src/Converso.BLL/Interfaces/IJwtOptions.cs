namespace SnapTalk.BLL.Interfaces;

public interface IJwtOptions
{
    public string JwtIssuer { get; init; }
    public string JwtAudience { get; init; }
    public string JwtSecretKey { get; init; }
    public int JwtExpiryInMinutes { get; init; }
    public int RefreshTokenExpiryInDays { get; init; }
}