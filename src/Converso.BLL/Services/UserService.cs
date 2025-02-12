using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnapTalk.BLL.Helpers;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SnapTalkContext _context;
    private readonly ICurrentContextProvider _currentContextProvider;
    private readonly IBlobService _blobService;
    private readonly BlobStorageSettings _blobStorageSettings;

    public UserService(UserManager<UserEntity> userManager, SnapTalkContext context,
        IJwtGeneratorService jwtGeneratorService, IJwtOptions jwtOptions,
        ICurrentContextProvider currentContextProvider, IBlobService blobService,
        BlobStorageSettings blobStorageSettings)
    {
        _userManager = userManager;
        _context = context;
        _currentContextProvider = currentContextProvider;
        _blobService = blobService;
        _blobStorageSettings = blobStorageSettings;
    }

    public async Task<Response<UpdateUserAvatarResponse>> UpdateUserAvatarAsync(IFormFile image)
    {
        if (image is null)
        {
            return ResponseErrors.ImageIsNotProvided;
        }

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == _currentContextProvider.CurrentUserId);

        if (currentUser is null)
        {
            return ResponseErrors.UserNotFound;
        }

        var uniqueFileName = FileNameHelper.CreateUniqueFileName(image.FileName);
        await _blobService.UploadFileBlobAsync(image.OpenReadStream(), image.ContentType, uniqueFileName);
        
        currentUser.ProfilePictureFileName = uniqueFileName;
        await _context.SaveChangesAsync();
        
        return new UpdateUserAvatarResponse(_blobService.GetBlobUrl(uniqueFileName));
    }

    public async Task<Response<bool>> CheckIsExistingEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return Response<bool>.Success(user is not null);
    }

    public async Task<Response<UserResponse>> GetUserAsync(Guid id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (user is null)
        {
            return ResponseErrors.UserNotFound;
        }

        return new UserResponse(user.Id, user.Email, user.UserName, user.FirstName, user.LastName, user.Bio,
            $"{_blobStorageSettings.BlobAccessUrl}/{user.ProfilePictureFileName}");
    }

    public async Task<Response<UserResponse>> UpdateUserInfoAsync(UpdateUserInfoRequest request)
    {
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == _currentContextProvider.CurrentUserId);

        if (currentUser is null)
        {
            return ResponseErrors.UserNotFound;
        }

        currentUser.FirstName = request.FirstName;
        currentUser.LastName = request.LastName;
        currentUser.Bio = request.Bio;
        currentUser.UserName = request.Username;

        await _context.SaveChangesAsync();

        return new UserResponse(currentUser.Id, currentUser.Email!, currentUser.UserName, currentUser.FirstName,
            currentUser.LastName!, currentUser.Bio!, $"{_blobStorageSettings.BlobAccessUrl}/{currentUser.ProfilePictureFileName}");
    }
}