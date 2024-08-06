using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SnapTalkContext _context;
    private readonly IJwtGeneratorService _jwtGeneratorService;
    private readonly IJwtOptions _jwtOptions;
    
    public AuthenticationService(
        UserManager<UserEntity> userManager,
        SnapTalkContext context,
        IJwtGeneratorService jwtGeneratorService,
        IJwtOptions jwtOptions)
    {
        _userManager = userManager;
        _context = context;
        _jwtGeneratorService = jwtGeneratorService;
        _jwtOptions = jwtOptions;
    }
    
    public async Task<TokenResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var user = new UserEntity
        {
            UserName = registerRequest.UserName,
            Email = registerRequest.Email
        };
        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        
        var jwtToken = _jwtGeneratorService.GenerateJwtToken(user.Id);
        var refreshToken = _jwtGeneratorService.GenerateRefreshToken();

        await _context.Sessions.AddAsync(new SessionEntity()
        {
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryInDays),
            UserId = user.Id,
            RefreshToken = refreshToken
        });

        await _context.SaveChangesAsync();

        return new TokenResponse(jwtToken, refreshToken);
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        
        if (!result)
        {
            throw new Exception("Invalid password");
        }
        
        var jwtToken = _jwtGeneratorService.GenerateJwtToken(user.Id);
        var refreshToken = _jwtGeneratorService.GenerateRefreshToken();
        
        await _context.Sessions.AddAsync(new SessionEntity()
        {
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryInDays),
            UserId = user.Id,
            RefreshToken = refreshToken
        });
        
        await _context.SaveChangesAsync();
        
        return new TokenResponse(jwtToken, refreshToken);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.RefreshToken == request.RefreshToken);

        if (session == null || session.IsExpired){
            throw new Exception("Session not found");}
        
        var newJwtToken = _jwtGeneratorService.GenerateJwtToken(session.UserId);
        var newRefreshToken = _jwtGeneratorService.GenerateRefreshToken();
        
        session.RefreshToken = newRefreshToken;
        session.ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryInDays);
        _context.Sessions.Update(session);
        
        await _context.SaveChangesAsync();
        
        return new TokenResponse(newJwtToken, newRefreshToken);
    }
}