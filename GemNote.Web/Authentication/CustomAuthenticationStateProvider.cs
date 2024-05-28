using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;

namespace GemNote.Web.Authentication;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
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

		// var token = await localStorageService.GetItemAsync<string>("authToken");
		// if (string.IsNullOrEmpty(token))
		// {
		// 	return _anonymous;
		// }
		//
		// httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		//
		// var claims = JwtParser.ParseClaimsFromJwt(token);
		// var identity = new ClaimsIdentity(claims, "jwt");
		// var user = new ClaimsPrincipal(identity);
		//
		// return new AuthenticationState(user);
	}

	public async Task NotifyUserAuthenticationAsync(string token)
	{
		ClaimsPrincipal user = new();
		if (!string.IsNullOrEmpty(token))
		{
			var userClaims = Generics.GetUserClaimsFromJwt(token);
			user = Generics.GetClaimsPrincipalFromClaims(userClaims);
			await localStorageService.SetItemAsStringAsync("authToken", token);
		}
		var authState = Task.FromResult(new AuthenticationState(user));
		NotifyAuthenticationStateChanged(authState);
	}

	public async Task NotifyUserLogoutAsync()
	{
		await localStorageService.RemoveItemAsync("authToken");
		var authState = Task.FromResult(new AuthenticationState(_anonymous));
		NotifyAuthenticationStateChanged(authState);
	}
}