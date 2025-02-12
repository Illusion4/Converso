using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnapTalk.BLL.Helpers;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Constants;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class AuthenticationService(UserManager<UserEntity> userManager,
        SnapTalkContext context,
        IJwtGeneratorService jwtGeneratorService,
        IJwtOptions jwtOptions,
        IEmailService emailService,
        IOtpService otpService,
        IAvatarService avatarService,
        IBlobService blobService)
    : IAuthenticationService
{
    public async Task<Response<TokenResponse>> RegisterAsync(RegisterRequest registerRequest)
    {
        var signupCode = await context.SignupCodes
            .FirstOrDefaultAsync(x => x.Email == registerRequest.Email);
        
        if (signupCode == null
            || !otpService.VerifyOtp(registerRequest.Code, signupCode.Code))
        {
            return ResponseErrors.InvalidOtpCode;
        }
        
        var randomColor = UserNameColors.GetRandomColor();

        var avatarBytes = avatarService.GenerateAvatar(registerRequest.FirstName, randomColor);
        var uniqueAvatarFileName = FileNameHelper.CreateUniqueFileName($"{registerRequest.FirstName}.png");
        await blobService.UploadFileBlobAsync(new MemoryStream(avatarBytes), "image/png", uniqueAvatarFileName);
        
        var user = new UserEntity
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            UserNameColor = randomColor,
            ProfilePictureFileName = uniqueAvatarFileName
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
        
        await context.Sessions.AddAsync(new SessionEntity()
        {
            ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpiryInDays),
            UserId = user.Id,
            RefreshToken = refreshToken
        });
            
        await context.SaveChangesAsync();
        return new TokenResponse(jwtToken, refreshToken);
    }

    public async Task<Response<TokenResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        
        if (user == null)
        {
            return ResponseErrors.InvalidCredentials;
        }
        
        var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        
        if (!result)
        {
            return ResponseErrors.InvalidCredentials;
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

        if (session == null){
            throw new Exception("Session not found");
        }
        if(session.IsExpired)
        {
            throw new Exception("Session is expired");
        }
        
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