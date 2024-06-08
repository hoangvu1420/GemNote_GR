using GemNote.API.DTOs.UnitDtos;
using GemNote.API.DTOs;

namespace GemNote.API.Services.Contracts;

public interface IUnitService
{
	Task<ApiResponse> GetUnitsAsync();
	Task<ApiResponse> GetUnitsBySectionIdAsync(int sectionId);
	Task<ApiResponse> GetUnitByIdAsync(int id);
	Task<ApiResponse> CreateUnitAsync(CreateUnitDto unitDto);
	Task<ApiResponse> UpdateUnitAsync(int unitId, UpdateUnitDto unitDto);
	Task<ApiResponse> DeleteUnitAsync(int id);
}