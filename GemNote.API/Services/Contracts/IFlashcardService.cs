using GemNote.API.DTOs.UnitDtos;
using GemNote.API.DTOs;
using GemNote.API.DTOs.FlashcardDtos;

namespace GemNote.API.Services.Contracts;

public interface IFlashcardService
{
	Task<ApiResponse> GetFlashcardsAsync();
	Task<ApiResponse> GetFlashcardsByUnitIdAsync(int unitId);
	Task<ApiResponse> GetFlashcardByIdAsync(int id);
	Task<ApiResponse> CreateFlashcardAsync(CreateFlashcardDto flashcardDto);
	Task<ApiResponse> UpdateFlashcardAsync(int flashcardId, UpdateFlashcardDto flashcardDto);
	Task<ApiResponse> DeleteFlashcardAsync(int id);
	Task<ApiResponse> GetDueFlashcardsByUserIdAsync(string userId);
}