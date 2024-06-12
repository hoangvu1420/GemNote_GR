
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using GemNote.Web.Services.Contracts;
using GemNote.Web.StaticDetails;
using GemNote.Web.ViewModels.RequestModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Authentication;

public class AuthenticationMessageHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Check for authToken and expirationDate in localStorage
		var authToken = await localStorageService.GetItemAsync<string>("authToken", cancellationToken);
		var expirationDate = await localStorageService.GetItemAsync<DateTime>("expirationDate", cancellationToken);

		switch (string.IsNullOrEmpty(authToken))
		{
			// If authToken and expirationDate are present and not expired
			case false when expirationDate > DateTime.Now:
				// Add authorization header to the request
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
				break;
			case false when expirationDate < DateTime.Now:
			{
				var refreshToken = await localStorageService.GetItemAsync<string>("refreshToken", cancellationToken);
				if (string.IsNullOrEmpty(refreshToken)) return await base.SendAsync(request, cancellationToken);
				var userId = await localStorageService.GetItemAsync<string>("userId", cancellationToken);

				// Call RefreshTokenAsync method to get new authToken
				var client = new HttpClient { BaseAddress = new Uri(ApiUri.DevelopmentUri) };

				var requestData = new RefreshTokenRequest
				{
					UserId = userId!,
					RefreshToken = refreshToken
				};
				var response = await client.PostAsJsonAsync("api/auth/refresh-token", requestData, cancellationToken);
				var refreshTokenResponse = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(cancellationToken);

				// Add authorization header to the request
				if (!refreshTokenResponse!.IsSucceed) return await base.SendAsync(request, cancellationToken);
		
				await localStorageService.SetItemAsync("authToken", refreshTokenResponse.Token, cancellationToken);
				await localStorageService.SetItemAsync("refreshToken", refreshTokenResponse.RefreshToken, cancellationToken);
				await localStorageService.SetItemAsync("expirationDate", refreshTokenResponse.ExpirationDate, cancellationToken);
		
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshTokenResponse.Token);
				break;
			}
		}
		return await base.SendAsync(request, cancellationToken);
	}
}