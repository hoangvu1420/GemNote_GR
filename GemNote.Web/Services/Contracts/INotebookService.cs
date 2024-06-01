using System.Net;
using GemNote.Web.ViewModels.NotebookViewModels;
using GemNote.Web.ViewModels.ResponseModels;

namespace GemNote.Web.Services.Contracts;

public interface INotebookService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebooksByUserIdAsync(string userId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetNotebookAsync(int notebookId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateNotebookAsync(CreateNotebookVm notebookVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateNotebookAsync(UpdateNotebookVm notebookVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteNotebookAsync(int notebookId);
}