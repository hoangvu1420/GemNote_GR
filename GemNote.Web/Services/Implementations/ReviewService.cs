using System.Net;
using System.Net.Http.Json;
using GemNote.Web.Services.Contracts;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.ReviewViewModels;

namespace GemNote.Web.Services.Implementations;

public class ReviewService(IHttpClientFactory httpClientFactory) : IReviewService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetReviewsByUserIdAsync(int userId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/review-sessions/user/{userId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these reviews."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get reviews."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting reviews. Please try again."];
						break;
				}

				return (new ApiResponse
				{
					IsSucceed = false,
					ErrorMessages = errorMessages
				}, statusCode);
			}

			var content = await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting reviews. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting reviews. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetDueReviewsByUserIdAsync(string userId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/review-sessions/due-reviews/{userId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these reviews."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get reviews."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting reviews. Please try again."];
						break;
				}

				return (new ApiResponse
				{
					IsSucceed = false,
					ErrorMessages = errorMessages
				}, statusCode);
			}

			var content = await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { "There was an error getting reviews. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting reviews. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetReviewsByUnitIdAsync(string unitId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/review-sessions/unit/{unitId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these reviews."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get reviews."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting reviews. Please try again."];
						break;
				}

				return (new ApiResponse
				{
					IsSucceed = false,
					ErrorMessages = errorMessages
				}, statusCode);
			}

			var content = await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { "There was an error getting reviews. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting reviews. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> CreateReviewSessionAsync(
		IEnumerable<CreateReviewVm> reviews)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/review-sessions/create-range-review", reviews);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to create these reviews."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to create reviews."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error creating reviews. Please try again."];
						break;
				}

				return (new ApiResponse
				{
					IsSucceed = false,
					ErrorMessages = errorMessages
				}, statusCode);
			}

			var content = await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { "There was an error creating reviews. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting reviews. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}
}