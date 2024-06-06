using AutoMapper;
using GemNote.API.DTOs;
using GemNote.API.DTOs.NotebookDtos;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Services.Contracts;

namespace GemNote.API.Services.Implementations;

public class NotebookService(
	INotebookRepository notebookRepository,
	ICategoryRepository categoryRepository,
	IMapper mapper) : INotebookService
{
	public async Task<ApiResponse> GetNotebooksAsync()
	{
		var response = new ApiResponse();

		var notebooks = await notebookRepository.GetAllAsync(includeProperties: "Category, Sections");

		var notebookList = notebooks.ToList();
		if (!notebookList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No notebooks found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<NotebookDto>>(notebookList);

		return response;
	}

	public async Task<ApiResponse> GetNotebooksByUserIdAsync(string userId)
	{
		var response = new ApiResponse();

		var notebooks =
			await notebookRepository.GetAllAsync(filter: n => n.AppUserId == userId, includeProperties: "Category, Sections");

		var notebookList = notebooks.ToList();
		if (!notebookList.Any())
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["No notebooks found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<IEnumerable<NotebookDto>>(notebookList);

		return response;
	}

	public async Task<ApiResponse> GetNotebookByIdAsync(int notebookId)
	{
		var response = new ApiResponse();

		var notebook =
			await notebookRepository.GetAsync(filter: n => n.Id == notebookId, includeProperties: "Category, Sections");

		if (notebook == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Notebook not found"];
			return response;
		}

		response.IsSucceed = true;
		response.Data = mapper.Map<NotebookDto>(notebook);

		return response;
	}

	public async Task<ApiResponse> CreateNotebookAsync(CreateNotebookDto notebookDto)
	{
		var response = new ApiResponse();

		var category = await categoryRepository.GetAsync(filter: c => c.Name == notebookDto.CategoryName);

		int categoryId;
		if (category == null)
		{
			var newCategory = new Category { Name = notebookDto.CategoryName };
			await categoryRepository.CreateAsync(newCategory);
			categoryId = newCategory.Id;
		}
		else
		{
			categoryId = category.Id;
		}

		var notebook = mapper.Map<Notebook>(notebookDto);
		notebook.CategoryId = categoryId;

		await notebookRepository.CreateAsync(notebook);

		response.IsSucceed = true;
		response.Data = mapper.Map<NotebookDto>(notebook);

		return response;
	}

	public async Task<ApiResponse> UpdateNotebookAsync(int notebookId, UpdateNotebookDto notebookDto)
	{
		var response = new ApiResponse();

		if (notebookId != notebookDto.Id)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["notebookId in the URL and in the request body do not match"];
			return response;
		}

		var notebook = await notebookRepository.GetAsync(filter: n => n.Id == notebookId, includeProperties: "Category, Sections");
		if (notebook == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Notebook not found"];
			return response;
		}

		int categoryId;
		if (notebook.Category != null && notebook.Category.Name != notebookDto.CategoryName)
		{
			var category = await categoryRepository.GetAsync(filter: c => c.Name == notebookDto.CategoryName);

			if (category == null)
			{
				var newCategory = new Category { Name = notebookDto.CategoryName };
				await categoryRepository.CreateAsync(newCategory);
				categoryId = newCategory.Id;
			}
			else
			{
				categoryId = category.Id;
			}
		}
		else
		{
			categoryId = notebook.CategoryId;
		}

		var notebookToUpdate = mapper.Map<Notebook>(notebookDto);
		notebookToUpdate.CategoryId = categoryId;

		var updatedNotebook = await notebookRepository.UpdateAsync(notebookToUpdate);

		response.IsSucceed = true;
		response.Data = mapper.Map<NotebookDto>(updatedNotebook);

		return response;
	}

	public async Task<ApiResponse> DeleteNotebookAsync(int notebookId)
	{
		var response = new ApiResponse();

		var notebook = await notebookRepository.GetAsync(filter: n => n.Id == notebookId);
		if (notebook == null)
		{
			response.IsSucceed = false;
			response.ErrorMessages = ["Notebook not found"];
			return response;
		}

		await notebookRepository.DeleteAsync(notebookId);

		response.IsSucceed = true;

		return response;
	}
}