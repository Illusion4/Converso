using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Interfaces;

public interface IAuthenticationService
{
    public Task<Response<TokenResponse>> RegisterAsync(RegisterRequest registerRequest);
    
    public Task<Response<TokenResponse>> LoginAsync(LoginRequest loginRequest);
    
    public Task<Response<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

    public Task SendVerifyEmailCodeAsync(SendVerifyEmailCodeRequest request);
}