using System.Net;
using System.Net.Http.Json;
using GemNote.Web.Services.Contracts;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.SectionViewModels;

namespace GemNote.Web.Services.Implementations;

public class SectionService(IHttpClientFactory httpClientFactory) : ISectionService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetSectionsByNotebookIdAsync(int notebookId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/sections?notebookId={notebookId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these sections."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get sections."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting sections. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting sections. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting sections. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public Task<(ApiResponse response, HttpStatusCode statusCode)> GetSectionAsync(int sectionId)
	{
		throw new NotImplementedException();
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> CreateSectionAsync(CreateSectionVm sectionVm)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/sections", sectionVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to create this section."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to create sections."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error creating section. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error creating section. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error creating section. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateSectionAsync(UpdateSectionVm sectionVm)
	{
		try
		{
			var response = await _httpClient.PutAsJsonAsync($"api/sections/{sectionVm.Id}", sectionVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to update this section."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to update sections."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error updating section. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error updating section. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error updating section. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteSectionAsync(int sectionId)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"api/sections/{sectionId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				var errorMessages = new List<string>();

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to delete this section."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to delete sections."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error deleting section. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error deleting section. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception e)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error deleting section. Please try again. {e.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}
}