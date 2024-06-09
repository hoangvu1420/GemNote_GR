using System.Net;
using System.Net.Http.Json;
using GemNote.Web.Services.Contracts;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.UnitViewModels;

namespace GemNote.Web.Services.Implementations;

public class UnitService(IHttpClientFactory httpClientFactory) : IUnitService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ServerApi");

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetUnitsBySectionIdAsync(int sectionId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/units?sectionId={sectionId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get these units."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get units."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting units. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting units. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting units. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> GetUnitAsync(int unitId)
	{
		try
		{
			var response = await _httpClient.GetAsync($"api/units/{unitId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to get this unit."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to get this unit."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error getting this unit. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error getting this unit. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error getting this unit. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> CreateUnitAsync(CreateUnitVm unitVm)
	{
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/units", unitVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to create this unit."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to create this unit."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error creating this unit. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error creating this unit. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error creating this unit. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateUnitAsync(UpdateUnitVm unitVm)
	{
		try
		{
			var response = await _httpClient.PutAsJsonAsync($"api/units/{unitVm.Id}", unitVm);

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to update this unit."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to update this unit."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error updating this unit. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error updating this unit. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error updating this unit. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}

	public async Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteUnitAsync(int unitId)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"api/units/{unitId}");

			if (!response.IsSuccessStatusCode)
			{
				var statusCode = response.StatusCode;
				List<string> errorMessages;

				switch (statusCode)
				{
					case HttpStatusCode.Forbidden:
						errorMessages = ["You are forbidden to delete this unit."];
						break;
					case HttpStatusCode.Unauthorized:
						errorMessages = ["You are not authorized to delete this unit."];
						break;
					case HttpStatusCode.NotFound:
						var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
						return (error!, statusCode);
					default:
						errorMessages = ["There was an error deleting this unit. Please try again."];
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
				ErrorMessages = new List<string> { "There was an error deleting this unit. Please try again." }
			};

			return (content, response.StatusCode);
		}
		catch (Exception ex)
		{
			return (new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = new List<string> { $"There was an error deleting this unit. Please try again. {ex.Message}" }
			}, HttpStatusCode.InternalServerError);
		}
	}
}