using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class JwtGeneratorService(IJwtOptions jwtOptions) : IJwtGeneratorService
{
    public string GenerateJwtToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.JwtSecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.JwtExpiryInMinutes),
            Issuer = jwtOptions.JwtIssuer,
            Audience = jwtOptions.JwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public Guid GenerateRefreshToken()
    {
        return Guid.NewGuid();
    }
}