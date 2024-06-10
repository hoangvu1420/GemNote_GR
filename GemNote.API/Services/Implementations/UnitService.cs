using AutoMapper;
using GemNote.API.DTOs;
using GemNote.API.DTOs.UnitDtos;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Services.Contracts;

namespace GemNote.API.Services.Implementations;

public class UnitService(
	IUnitRepository unitRepository,
	ISectionRepository sectionRepository,
	IMapper mapper) : IUnitService
{
	public async Task<ApiResponse> GetUnitsAsync()
	{
		var response = new ApiResponse();

		var units = await unitRepository.GetAllAsync(includeProperties: "Flashcards");

		var unitList = units.ToList();
		if (!unitList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No units found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<UnitDto>>(unitList);

		return response;
	}

	public async Task<ApiResponse> GetUnitsBySectionIdAsync(int sectionId)
	{
		var response = new ApiResponse();

		var units = await unitRepository.GetAllAsync(filter: u => u.SectionId == sectionId, includeProperties: "Flashcards");

		var unitList = units.ToList();
		if (!unitList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No units found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<UnitDto>>(unitList);

		return response;
	}

	public async Task<ApiResponse> GetUnitByIdAsync(int id)
	{
		var response = new ApiResponse();

		var unit = await unitRepository.GetAsync(filter: u => u.Id == id, includeProperties: "Section, Section.Notebook, Flashcards");

		if (unit == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Unit not found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<DetailedUnitDto>(unit);

		return response;
	}

	public async Task<ApiResponse> CreateUnitAsync(CreateUnitDto unitDto)
	{
		var response = new ApiResponse();

		var unit = mapper.Map<Unit>(unitDto);
		if (!await sectionRepository.ExistsAsync(u => u.Id == unit.SectionId))
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Section not found"];
			return response;
		}

		await unitRepository.CreateAsync(unit);

		var createdUnit = await unitRepository.GetAsync(filter: u => u.Id == unit.Id, includeProperties: "Section");

		response.IsSucceed = true;
		response.Data = mapper.Map<UnitDto>(createdUnit);

		return response;
	}

	public async Task<ApiResponse> UpdateUnitAsync(int unitId, UpdateUnitDto unitDto)
	{
		var response = new ApiResponse();

		if (unitId != unitDto.Id)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Unit ID mismatch"];
			return response;
		}

		var unit = await unitRepository.GetAsync(filter: u => u.Id == unitId, includeProperties: "Section, Flashcards");

		if (unit == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Unit not found"];
			return response;
		}

		var unitToUpdate = mapper.Map<Unit>(unitDto);
		unitToUpdate.SectionId = unit.SectionId;

		var updatedUnit = await unitRepository.UpdateAsync(unitToUpdate);

		response.IsSucceed = true;
		response.Data = mapper.Map<UnitDto>(updatedUnit);

		return response;
	}

	public async Task<ApiResponse> DeleteUnitAsync(int id)
	{
		var response = new ApiResponse();

		var unit = await unitRepository.GetAsync(filter: u => u.Id == id);
		if (unit == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Unit not found"];
			return response;
		}

		await unitRepository.DeleteAsync(id);

		response.IsSucceed = true;

		return response;
	}
}