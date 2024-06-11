using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GemNote.Web.Authentication;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
	: AuthenticationStateProvider
{
	private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var token = await localStorageService.GetItemAsStringAsync("authToken");
			if (string.IsNullOrEmpty(token))
			{
				return await Task.FromResult(new AuthenticationState(_anonymous));
			}

			var claims = Generics.GetUserClaimsFromJwt(token);

			var claimsPrincipal = Generics.GetClaimsPrincipalFromClaims(claims);

			return new AuthenticationState(claimsPrincipal);
		}
		catch
		{
			return await Task.FromResult(new AuthenticationState(_anonymous));
		}
	}

	public async Task NotifyUserAuthenticationAsync(string token, DateTime expirationDate, string refreshToken)
	{
		ClaimsPrincipal user = new();
		if (!string.IsNullOrEmpty(token))
		{
			var userClaims = Generics.GetUserClaimsFromJwt(token);
			user = Generics.GetClaimsPrincipalFromClaims(userClaims);
			await localStorageService.SetItemAsStringAsync("authToken", token);
		}
		if (expirationDate != default)
		{
			await localStorageService.SetItemAsync("expirationDate", expirationDate);
		}
		if (!string.IsNullOrEmpty(refreshToken))
		{
			await localStorageService.SetItemAsStringAsync("refreshToken", refreshToken);
		}
		var authState = Task.FromResult(new AuthenticationState(user));
		NotifyAuthenticationStateChanged(authState);
	}

	public async Task NotifyUserLogoutAsync()
	{
		await localStorageService.RemoveItemAsync("authToken");
		await localStorageService.RemoveItemAsync("expirationDate");
		await localStorageService.RemoveItemAsync("refreshToken");

		var authState = Task.FromResult(new AuthenticationState(_anonymous));
		NotifyAuthenticationStateChanged(authState);
	}
}