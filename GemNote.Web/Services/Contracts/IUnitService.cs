using GemNote.Web.ViewModels.SectionViewModels;
using System.Net;
using GemNote.Web.ViewModels.ResponseModels;
using GemNote.Web.ViewModels.UnitViewModels;

namespace GemNote.Web.Services.Contracts;

public interface IUnitService
{
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetUnitsBySectionIdAsync(int sectionId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> GetUnitAsync(int unitId);
	Task<(ApiResponse response, HttpStatusCode statusCode)> CreateUnitAsync(CreateUnitVm unitVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> UpdateUnitAsync(UpdateUnitVm unitVm);
	Task<(ApiResponse response, HttpStatusCode statusCode)> DeleteUnitAsync(int unitId);
}