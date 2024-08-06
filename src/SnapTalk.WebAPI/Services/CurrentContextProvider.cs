using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.WebAPI.Services;

public class CurrentContextProvider(IHttpContextAccessor httpContextAccessor) : ICurrentContextProvider
{
    public Guid CurrentUserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User
                .FindFirst(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            
            if (value is null)
                throw new Exception("User is not authenticated");
            
            return Guid.Parse(value);
        }
    }
}