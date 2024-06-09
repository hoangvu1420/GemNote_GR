using AutoMapper;
using GemNote.API.DTOs;
using GemNote.API.DTOs.SectionDtos;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Services.Contracts;

namespace GemNote.API.Services.Implementations;

public class SectionService(
	ISectionRepository sectionRepository,
	IMapper mapper) : ISectionService
{
	public async Task<ApiResponse> GetSectionsAsync()
	{
		var response = new ApiResponse();

		var sections = await sectionRepository.GetAllAsync(includeProperties: "Notebook, Units");

		var sectionList = sections.ToList();
		if (!sectionList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No sections found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<SectionDto>>(sectionList);

		return response;
	}

	public async Task<ApiResponse> GetSectionsByNotebookIdAsync(int notebookId)
	{
		var response = new ApiResponse();

		var sections = await sectionRepository.GetAllAsync(filter: s => s.NotebookId == notebookId, includeProperties: "Notebook, Units");

		var sectionList = sections.ToList();
		if (!sectionList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No sections found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<SectionDto>>(sectionList);

		return response;
	}

	public async Task<ApiResponse> GetSectionByIdAsync(int sectionId)
	{
		var response = new ApiResponse();

		var section = await sectionRepository.GetAsync(filter: s => s.Id == sectionId);

		if (section == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Section not found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<SectionDto>(section);

		return response;
	}

	public async Task<ApiResponse> CreateSectionAsync(CreateSectionDto sectionDto)
	{
		var response = new ApiResponse();

		var section = mapper.Map<Section>(sectionDto);

		await sectionRepository.CreateAsync(section);

		var createdSection = await sectionRepository.GetAsync(filter: s => s.Id == section.Id, includeProperties: "Notebook");

		response.IsSucceed = true;
		response.Data = mapper.Map<SectionDto>(createdSection);

		return response;
	}

	public async Task<ApiResponse> UpdateSectionAsync(int sectionId, UpdateSectionDto sectionDto)
	{
		var response = new ApiResponse();

		if (sectionId != sectionDto.Id)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["sectionId in the URL and in the request body do not match"];
			return response;
		}

		var section = await sectionRepository.GetAsync(filter: s => s.Id == sectionId, includeProperties: "Notebook, Units");

		if (section == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Section not found"];
			return response;
		}

		var sectionToUpdate = mapper.Map<Section>(sectionDto);
		sectionToUpdate.NotebookId = section.NotebookId;

		var updatedSection = await sectionRepository.UpdateAsync(sectionToUpdate);

		response.IsSucceed = true;
		response.Data = mapper.Map<SectionDto>(updatedSection);

		return response;
	}

	public async Task<ApiResponse> DeleteSectionAsync(int id)
	{
		var response = new ApiResponse();

		var section = await sectionRepository.GetAsync(filter: s => s.Id == id);
		if (section == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Section not found"];
			return response;
		}

		await sectionRepository.DeleteAsync(id);

		response.IsSucceed = true;

		return response;
	}
}