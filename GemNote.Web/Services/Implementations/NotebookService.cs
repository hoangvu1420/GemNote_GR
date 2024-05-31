using System.Net;
using System.Net.Http.Json;
using GemNote.Web.Services.Contracts;
using GemNote.Web.States;
using GemNote.Web.ViewModels.ResourceModels;
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
				var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
				var statusCode = response.StatusCode;
				logger.LogError($"Error getting notebooks: {statusCode}");
				return (error!, statusCode);
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

	public Task<(ApiResponse response, HttpStatusCode statusCode)> CreateNotebookAsync(NotebookVm notebookVm)
	{
		throw new NotImplementedException();
	}

	public Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateNotebookAsync(NotebookVm notebookVm)
	{
		throw new NotImplementedException();
	}

	public Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteNotebookAsync(int notebookId)
	{
		throw new NotImplementedException();
	}
}