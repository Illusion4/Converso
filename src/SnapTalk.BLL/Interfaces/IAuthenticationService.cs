using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Interfaces;

public interface IAuthenticationService
{
    public Task<TokenResponse> RegisterAsync(RegisterRequest registerRequest);
    
    public Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
    
    public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
}