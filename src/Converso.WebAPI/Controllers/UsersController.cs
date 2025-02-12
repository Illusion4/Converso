using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Controllers;

[Route("api/users")]
[Authorize]
[ApiController]
public class UsersController(IUserService userService): ControllerBase
{
    [HttpGet("is-existing-email")]
    [AllowAnonymous]
    public async Task<ActionResult> CheckEmailAsync([FromQuery] string email)
    {
        var isEmailRegistered = await userService.CheckIsExistingEmailAsync(email);

        return Ok(isEmailRegistered);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserAsync(Guid id)
    {
        var user = await userService.GetUserAsync(id);

        return Ok(user);
    }
    
    [HttpPut("update-avatar")]
    public async Task<ActionResult> UpdateAvatarAsync([FromForm] IFormFile image)
    {
        var response = await userService.UpdateUserAvatarAsync(image);

        return Ok(response);
    }
    
    [HttpPut("update-info")]
    public async Task<ActionResult> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest request)
    {
        var response = await userService.UpdateUserInfoAsync(request);

        return Ok(response);
    }
}