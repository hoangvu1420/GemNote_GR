using Blazored.LocalStorage;

namespace GemNote.Web.States;

public class UserState(ILocalStorageService localStorageService)
{
	private string? _userId;
	private string? _userFullName;
	private string? _avatarUrl;
	private bool _isAuthenticated;
	private bool _isAdmin;
	private bool _isRememberMe;

	public string? UserId
	{
		get => _userId;
		set
		{
			if (_userId == value) return;
			_userId = value;
			NotifyStateChanged();
		}
	}

	public string? UserFullName
	{
		get => _userFullName;
		set
		{
			if (_userFullName == value) return;
			_userFullName = value;
			NotifyStateChanged();
		}
	}

	public string? AvatarUrl
	{
		get => _avatarUrl;
		set
		{
			if (_avatarUrl == value) return;
			_avatarUrl = value;
			NotifyStateChanged();
		}
	}

	public bool IsAuthenticated
	{
		get => _isAuthenticated;
		set
		{
			if (_isAuthenticated == value) return;
			_isAuthenticated = value;
			NotifyStateChanged();
		}
	}

	public bool IsAdmin
	{
		get => _isAdmin;
		set
		{
			if (_isAdmin == value) return;
			_isAdmin = value;
			NotifyStateChanged();
		}
	}

	public bool IsRememberMe
	{
		get => _isRememberMe;
		set
		{
			if (_isRememberMe == value) return;
			_isRememberMe = value;
			NotifyStateChanged();
		}
	}

	public event Action OnChange;

	private void NotifyStateChanged() => OnChange?.Invoke();

	public async Task LoadStateAsync()
	{
		UserId = await localStorageService.GetItemAsync<string>("userId");
		UserFullName = await localStorageService.GetItemAsync<string>("userFullName");
		AvatarUrl = await localStorageService.GetItemAsync<string>("avatar");
		IsAuthenticated = !string.IsNullOrEmpty(UserId);
		IsAdmin = await localStorageService.GetItemAsync<bool>("isAdmin");
		IsRememberMe = await localStorageService.GetItemAsync<bool>("isRememberMe");
	}

	public async Task SaveStateAsync()
	{
		await localStorageService.SetItemAsync("userId", UserId);
		await localStorageService.SetItemAsync("avatar", AvatarUrl);
		await localStorageService.SetItemAsync("userFullName", UserFullName);
		await localStorageService.SetItemAsync("isAdmin", IsAdmin);
		await localStorageService.SetItemAsync("isRememberMe", IsRememberMe);
	}

	public async Task ClearStateAsync()
	{
		UserId = null;
		UserFullName = null;
		AvatarUrl = null;
		IsAuthenticated = false;
		IsAdmin = false;
		IsRememberMe = false;
		await localStorageService.RemoveItemAsync("userId");
		await localStorageService.RemoveItemAsync("avatar");
		await localStorageService.RemoveItemAsync("userFullName");
		await localStorageService.RemoveItemAsync("isAdmin");
		await localStorageService.RemoveItemAsync("isRememberMe");
	}
}