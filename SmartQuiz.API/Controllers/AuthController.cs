using Microsoft.AspNetCore.Mvc;
using SmartQuiz.Application.Interfaces.Services;
using SmartQuiz.Shared.Dtos.Auth;

namespace SmartQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromQuery] string refreshToken, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
        return Ok(response);
    }
}
