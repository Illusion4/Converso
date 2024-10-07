using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class AuthenticationService(UserManager<UserEntity> userManager,
        SnapTalkContext context,
        IJwtGeneratorService jwtGeneratorService,
        IJwtOptions jwtOptions,
        IEmailService emailService,
        IOtpService otpService)
    : IAuthenticationService
{
    public async Task<Response<TokenResponse>> RegisterAsync(RegisterRequest registerRequest)
    {
        var signupCode = await context.SignupCodes
            .FirstOrDefaultAsync(x => x.Email == registerRequest.Email);
        
        if (signupCode == null
            || !otpService.VerifyOtp(registerRequest.Code, signupCode.Code))
        {
            return AuthenticationError.InvalidOtpCode;
        }
        
        var user = new UserEntity
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            UserName = registerRequest.UserName,
            Email = registerRequest.Email,
        };
        var result = await userManager.CreateAsync(user, registerRequest.Password);
        
        //TODO: Add error handling
        if (!result.Succeeded)
        {
            throw new Exception( string.Join("; ", result.Errors.Select(x => x.Description).ToArray()));
        }
        
        var jwtToken = jwtGeneratorService.GenerateJwtToken(user.Id);
        var refreshToken = jwtGeneratorService.GenerateRefreshToken();
        context.SignupCodes.Remove(signupCode);
            
        await context.SaveChangesAsync();
        return new TokenResponse(jwtToken, refreshToken);
    }

    public async Task<Response<TokenResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        
        if (!result)
        {
            throw new Exception("Invalid password");
        }
        
        var jwtToken = jwtGeneratorService.GenerateJwtToken(user.Id);
        var refreshToken = jwtGeneratorService.GenerateRefreshToken();
        
        await context.Sessions.AddAsync(new SessionEntity()
        {
            ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpiryInDays),
            UserId = user.Id,
            RefreshToken = refreshToken
        });
        
        await context.SaveChangesAsync();
        
        return new TokenResponse(jwtToken, refreshToken);
    }

    public async Task<Response<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var session = await context.Sessions
            .FirstOrDefaultAsync(s => s.RefreshToken == request.RefreshToken);

        if (session == null || session.IsExpired){
            throw new Exception("Session not found");}
        
        var newJwtToken = jwtGeneratorService.GenerateJwtToken(session.UserId);
        var newRefreshToken = jwtGeneratorService.GenerateRefreshToken();
        
        session.RefreshToken = newRefreshToken;
        session.ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpiryInDays);
        context.Sessions.Update(session);
        
        await context.SaveChangesAsync();
        
        return new TokenResponse(newJwtToken, newRefreshToken);
    }

    public async Task SendVerifyEmailCodeAsync(SendVerifyEmailCodeRequest request)
    {
        var code = otpService.GenerateOtp();
        var hashedCode = otpService.HashOtp(code);
        
        var signupCode = new SignupCodeEntity
        {
            Email = request.Email,
            Code = hashedCode
        };

        var isEmailIsUsed = await context.SignupCodes.AnyAsync(x => x.Email == request.Email);

        if (isEmailIsUsed)
        {
            context.SignupCodes.Update(signupCode);
        }
        else
        {
            await context.SignupCodes.AddAsync(signupCode);
        }
        
        await context.SaveChangesAsync();
        
        await emailService.SendEmailAsync(request.Email, "Verify Email", $"Your code is {code}");
    }
}