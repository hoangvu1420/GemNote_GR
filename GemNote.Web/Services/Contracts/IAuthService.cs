using GemNote.Web.ViewModels.RequestModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Contracts;

public interface IAuthService
{
	Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
	Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
	Task LogoutAsync();
}