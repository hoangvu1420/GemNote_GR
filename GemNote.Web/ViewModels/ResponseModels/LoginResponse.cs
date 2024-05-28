using GemNote.Web.ViewModels.UserModel;

namespace GemNote.Web.ViewModels.ResponseModels;

public class LoginResponse
{
	public bool IsSucceed { get; init; }
	public List<string>? ErrorMessages { get; init; }
	public string? Token { get; init; }
	public DateTime ExpirationDate { get; init; }
	public string? RefreshToken { get; init; }
	public UserVm? UserInfo { get; init; }
}