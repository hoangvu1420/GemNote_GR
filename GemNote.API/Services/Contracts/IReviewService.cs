using GemNote.API.DTOs;
using GemNote.API.DTOs.ReviewDtos;

namespace GemNote.API.Services.Contracts;

public interface IReviewService
{
	Task<ApiResponse> CreateReviewAsync(CreateReviewDto reviewDto);
	Task<ApiResponse> DeleteReviewAsync(int reviewId);
	Task<ApiResponse> CreateRangeReviewAsync(IEnumerable<CreateReviewDto> reviewDtos);
	Task<ApiResponse> DeleteRangeReviewAsync(IEnumerable<int> reviewIds);
	Task<ApiResponse> GetDueReviewsByUserId(string userId);
	Task<ApiResponse> GetReviewsByUserId(string userId);
	Task<ApiResponse> GetReviewsByFlashcardId(int flashcardId);
	Task<ApiResponse> GetReviewsByUnitId(int unitId);
}