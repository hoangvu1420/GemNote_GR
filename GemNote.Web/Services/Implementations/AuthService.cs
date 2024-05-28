using Blazored.LocalStorage;
using GemNote.Web.Services.Contracts;
using GemNote.Web.ViewModels.RequestModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using GemNote.Web.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Implementations;

public class AuthService(
	IHttpClientFactory httpClientFactory,
	ILocalStorageService localStorageService,
	AuthenticationStateProvider authenticationStateProvider)
	: IAuthService
{
	private readonly HttpClient _apiClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
	{
		var response = await _apiClient.PostAsJsonAsync("api/auth/login", loginRequest);
		
		var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
		if (loginResponse == null)
			return new LoginResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { "There was an error logging in. Please try again." }
			};

		await localStorageService.SetItemAsync("authToken", loginResponse.Token);
		
		await ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthenticationAsync(loginResponse.Token!);
		
		// _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
		
		return loginResponse;
	}

	public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
	{
		var response = await _apiClient.PostAsJsonAsync("api/auth/register", registerRequest);

		var result = await response.Content.ReadAsStringAsync();
		var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

		return authResponse!;
	}

	public async Task LogoutAsync()
	{
		await ((CustomAuthenticationStateProvider)authenticationStateProvider).NotifyUserLogoutAsync();
		_apiClient.DefaultRequestHeaders.Authorization = null;
	}
}