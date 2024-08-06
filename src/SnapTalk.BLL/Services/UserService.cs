using Microsoft.AspNetCore.Identity;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SnapTalkContext _context;
    private readonly IJwtGeneratorService _jwtGeneratorService;
    private readonly IJwtOptions _jwtOptions;

    UserService(UserManager<UserEntity> userManager, SnapTalkContext context,
        IJwtGeneratorService jwtGeneratorService, IJwtOptions jwtOptions)
    {
        _userManager = userManager;
        _context = context;
        _jwtGeneratorService = jwtGeneratorService;
        _jwtOptions = jwtOptions;
    }
}