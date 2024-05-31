using System.Net;
using GemNote.Web.ViewModels.ResourceModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Contracts;

public interface INotebookService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebooksByUserIdAsync(string userId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebookAsync(int notebookId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateNotebookAsync(NotebookVm notebookVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateNotebookAsync(NotebookVm notebookVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteNotebookAsync(int notebookId);
}