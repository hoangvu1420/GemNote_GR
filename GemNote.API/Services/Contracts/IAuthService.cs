using GemNote.API.DTOs.AuthDtos;

namespace GemNote.API.Services.Contracts;

public interface IAuthService
{
	Task<AuthResponse> RegisterAsync(RegisterRequestDto request);
	Task<LoginResponse> LoginAsync(LoginRequestDto request);
	Task<RefreshTokenResponse> IssueRefreshTokenAsync(RefreshTokenRequestDto request);
	Task<AuthResponse> AddToRoleAsync(AddToRoleDto request);
}