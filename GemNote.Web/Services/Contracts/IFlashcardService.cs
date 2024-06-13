using System.Net;
using GemNote.Web.ViewModels.FlashcardViewModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Contracts;

public interface IFlashcardService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetFlashcardsByUnitIdAsync(int unitId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetFlashcardAsync(int flashcardId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetDueFlashcardsByUserIdAsync(string userId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateFlashcardAsync(CreateFlashcardVm flashcardVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateFlashcardAsync(UpdateFlashcardVm flashcardVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteFlashcardAsync(int flashcardId);
}