using Microsoft.AspNetCore.Mvc;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController(ICurrentContextProvider currentContextProvider,
        IAuthenticationService authenticationService)
    : ControllerBase
{
    private readonly ICurrentContextProvider _currentContextProvider = currentContextProvider;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authenticationService.RegisterAsync(request);
        
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Error);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authenticationService.LoginAsync(request);
        
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Error);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await authenticationService.RefreshTokenAsync(request);
        
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Error);
    }
    
    [HttpPost("send-verify-email-code")]
    public async Task<IActionResult> SendVerifyEmailCode([FromBody] SendVerifyEmailCodeRequest request)
    {
        await authenticationService.SendVerifyEmailCodeAsync(request);
        
        return Ok();
    }
}