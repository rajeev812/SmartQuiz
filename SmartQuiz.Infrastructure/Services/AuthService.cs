using SmartQuiz.Application.Interfaces.Services;
using SmartQuiz.Domain.Entities;
using SmartQuiz.Domain.Enums;
using SmartQuiz.Shared.Dtos.Auth;

namespace SmartQuiz.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly JsonUserStore _userStore;

    public AuthService(JwtTokenService jwtTokenService, JsonUserStore userStore)
    {
        _jwtTokenService = jwtTokenService;
        _userStore = userStore;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException("Name, email and password are required.");
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Student,
            IsActive = true
        };

        await _userStore.AddAsync(user, cancellationToken);
        return CreateResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userStore.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        return CreateResponse(user);
    }

    public Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Refresh tokens are not available in JSON database mode.");
    }

    private AuthResponseDto CreateResponse(User user)
    {
        return new AuthResponseDto
        {
            Token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email, user.Role.ToString()),
            RefreshToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
