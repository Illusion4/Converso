using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class CurrentContextProvider(IHttpContextAccessor httpContextAccessor) : ICurrentContextProvider
{
    public Guid CurrentUserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User
                .FindFirstValue(JwtRegisteredClaimNames.Jti);
            
            return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
        }
    }
}