using GemNote.API.DTOs;
using GemNote.API.DTOs.SectionDtos;

namespace GemNote.API.Services.Contracts;

public interface ISectionService
{
	Task<ApiResponse> GetSectionsAsync();
	Task<ApiResponse> GetSectionsByNotebookIdAsync(int notebookId);
	Task<ApiResponse> GetSectionByIdAsync(int id);
	Task<ApiResponse> CreateSectionAsync(CreateSectionDto sectionDto);
	Task<ApiResponse> UpdateSectionAsync(int sectionId, UpdateSectionDto sectionDto);
	Task<ApiResponse> DeleteSectionAsync(int id);
}