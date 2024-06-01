using System.Net;
using System.Net.Http.Json;
using GemNote.Web.Services.Contracts;
using GemNote.Web.States;
using GemNote.Web.ViewModels.NotebookViewModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Implementations;

public class NotebookService(IHttpClientFactory httpClientFactory, ILogger<NotebookService> logger) : INotebookService
{
	private readonly HttpClient _apiClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebooksByUserIdAsync(string userId)
	{
		try
		{
			var response = await _apiClient.GetAsync($"api/notebooks?userId={userId}");
			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();
				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these notebooks."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get notebooks."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting notebooks. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting notebooks. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting notebooks. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebookAsync(int notebookId)
	{
		throw new NotImplementedException();
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> CreateNotebookAsync(CreateNotebookVm notebookVm)
	{
		try
		{
			var response = await _apiClient.PostAsJsonAsync("api/notebooks", notebookVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();
				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to create a notebook."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to create a notebook."];
						break;
					case HttpStatusCode.BadRequest:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error creating notebook. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error creating notebook. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error creating notebook. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateNotebookAsync(UpdateNotebookVm notebookVm)
	{
		try
		{
			var response = await _apiClient.PutAsJsonAsync($"api/notebooks/{notebookVm.Id}", notebookVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();
				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to update this notebook."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to update this notebook."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error updating notebook. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error updating notebook. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error updating notebook. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteNotebookAsync(int notebookId)
	{
		try
		{
			var response = await _apiClient.DeleteAsync($"api/notebooks/{notebookId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();
				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to delete this notebook."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to delete this notebook."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error deleting notebook. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error deleting notebook. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error deleting notebook. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}
}