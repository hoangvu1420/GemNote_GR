using System.Net;
using GemNote.Web.ViewModels.FlashcardViewModels;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.ReviewViewModels;

namespace GemNote.Web.Services.Contracts;

public interface IReviewService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetReviewsByUserIdAsync(int userId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetDueReviewsByUserIdAsync(string userId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetReviewsByUnitIdAsync(string unitId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateReviewSessionAsync(
		IEnumerable<CreateReviewVm> reviews);
}