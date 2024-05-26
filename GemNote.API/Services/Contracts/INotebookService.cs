using GemNote.API.DTOs;
using GemNote.API.DTOs.NotebookDtos;
using GemNote.API.Models;

namespace GemNote.API.Services.Contracts;

public interface INotebookService
{
	Task<ApiResponse> GetNotebooksAsync();
	Task<ApiResponse> GetNotebooksByUserIdAsync(string userId);
	Task<ApiResponse> GetNotebookByIdAsync(int notebookId);
	Task<ApiResponse> CreateNotebookAsync(CreateNotebookDto notebook);
	Task<ApiResponse> UpdateNotebookAsync(int notebookId, UpdateNotebookDto notebookDto);
	Task<ApiResponse> DeleteNotebookAsync(int notebookId);
}