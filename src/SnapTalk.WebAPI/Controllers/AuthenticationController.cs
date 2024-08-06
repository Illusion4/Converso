using Microsoft.AspNetCore.Mvc;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ICurrentContextProvider _currentContextProvider;
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticationController(ICurrentContextProvider currentContextProvider, IAuthenticationService authenticationService)
    {
        _currentContextProvider = currentContextProvider;
        _authenticationService = authenticationService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authenticationService.RegisterAsync(request);
        
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authenticationService.LoginAsync(request);
        
        return Ok(result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authenticationService.RefreshTokenAsync(request);
        
        return Ok(result);
    }
}