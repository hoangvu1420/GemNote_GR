
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace GemNote.Web.Authentication;

public class AuthenticationMessageHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Check for authToken and expirationDate in localStorage
		var authToken = await localStorageService.GetItemAsync<string>("authToken", cancellationToken);
		var expirationDate = await localStorageService.GetItemAsync<DateTime>("expirationDate", cancellationToken);

		// If authToken and expirationDate are present and not expired
		if (!string.IsNullOrEmpty(authToken) && expirationDate > DateTime.Now)
		{
			// Add authorization header to the request
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}