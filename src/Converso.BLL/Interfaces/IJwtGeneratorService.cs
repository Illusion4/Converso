namespace SnapTalk.BLL.Interfaces;

public interface IJwtGeneratorService
{
    public string GenerateJwtToken(Guid userId);
    
    public Guid GenerateRefreshToken();
}