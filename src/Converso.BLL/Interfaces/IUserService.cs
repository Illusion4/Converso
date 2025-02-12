using Microsoft.AspNetCore.Http;
using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Interfaces;

public interface IUserService
{
    Task<Response<UpdateUserAvatarResponse>> UpdateUserAvatarAsync(IFormFile file);
    
    Task<Response<bool>> CheckIsExistingEmailAsync(string email);
    
    Task<Response<UserResponse>> GetUserAsync(Guid id);
    
    Task<Response<UserResponse>> UpdateUserInfoAsync(UpdateUserInfoRequest request);
}