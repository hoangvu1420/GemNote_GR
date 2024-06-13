using GemNote.Web.Services.Contracts;
using GemNote.Web.ViewModels.UnitViewModels;
using System.Net;
using GemNote.Web.ViewModels.FlashcardViewModels;
using GemNote.Web.ViewModels.ResponseModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace GemNote.Web.Services.Implementations;

public class FlashcardService(IHttpClientFactory httpClientFactory) : IFlashcardService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetFlashcardsByUnitIdAsync(int unitId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/flashcards?unitId={unitId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these flashcards."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get flashcards."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting flashcards. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting flashcards. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting flashcards. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetFlashcardAsync(int flashcardId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/flashcards/{flashcardId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get this flashcard."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get this flashcard."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting this flashcard. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting this flashcard. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting this flashcard. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetDueFlashcardsByUserIdAsync(string userId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/flashcards/due/{userId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these flashcards."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get flashcards."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting flashcards. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting flashcards. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting flashcards. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> CreateFlashcardAsync(CreateFlashcardVm flashcardVm)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/flashcards", flashcardVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to create this flashcard."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to create this flashcard."];
						break;
					case HttpStatusCode.BadRequest:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error creating this flashcard. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error creating this flashcard. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error creating this flashcard. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateFlashcardAsync(UpdateFlashcardVm flashcardVm)
	{
		try
		{
			var response = await _httpClient.PutAsJsonAsync($"api/flashcards/{flashcardVm.Id}", flashcardVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to update this flashcard."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to update this flashcard."];
						break;
					case HttpStatusCode.BadRequest:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error updating this flashcard. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error updating this flashcard. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error updating this flashcard. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteFlashcardAsync(int flashcardId)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"api/flashcards/{flashcardId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to delete this flashcard."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to delete this flashcard."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error deleting this flashcard. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error deleting this flashcard. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error deleting this flashcard. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}
}