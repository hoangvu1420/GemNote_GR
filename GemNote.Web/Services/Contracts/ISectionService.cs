using GemNote.Web.ViewModels.NotebookViewModels;
using System.Net;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.SectionViewModels;

namespace GemNote.Web.Services.Contracts;

public interface ISectionService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetSectionsByNotebookIdAsync(int notebookId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetSectionAsync(int sectionId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateSectionAsync(CreateSectionVm sectionVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateSectionAsync(UpdateSectionVm sectionVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteSectionAsync(int sectionId);
}