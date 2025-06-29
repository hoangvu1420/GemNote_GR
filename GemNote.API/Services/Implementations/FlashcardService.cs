using AutoMapper;
using GemNote.API.DTOs;
using GemNote.API.DTOs.FlashcardDtos;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Services.Contracts;

namespace GemNote.API.Services.Implementations;

public class FlashcardService(
	IFlashcardRepository flashcardRepository,
	IReviewRepository reviewRepository,
	IMapper mapper) : IFlashcardService
{
	public async Task<ApiResponse> GetFlashcardsAsync()
	{
		var response = new ApiResponse();

		try
		{
			var flashcards = await flashcardRepository.GetAllAsync(includeProperties: "Unit");

			var flashcardList = flashcards.ToList();
			if (!flashcardList.Any())
			{
				response.IsSucceed= false;
				response.ErrorMessages = ["No flashcards found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<FlashcardDto>>(flashcardList);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> GetFlashcardsByUnitIdAsync(int unitId)
	{
		var response = new ApiResponse();

		try
		{
			var flashcards = await flashcardRepository.GetAllAsync(filter: f => f.UnitId == unitId, includeProperties: "Unit");

			var flashcardList = flashcards.ToList();
			if (!flashcardList.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No flashcards found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<FlashcardDto>>(flashcardList);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> GetFlashcardByIdAsync(int id)
	{
		var response = new ApiResponse();

		try
		{
			var flashcard = await flashcardRepository.GetAsync(filter: f => f.Id == id, includeProperties: "Unit");

			if (flashcard == null)
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Flashcard not found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<FlashcardDto>(flashcard);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> GetDueFlashcardsByUserIdAsync(string userId)
	{
		var response = new ApiResponse();

		try
		{
			// Get all reviews for the user
			var reviews = await reviewRepository.GetAllAsync(
				filter: r => r.AppUserId == userId,
				includeProperties: "Flashcard, Flashcard.Unit"
			);

			// Group by FlashcardId and select the review with the latest ReviewDate for each group
			var latestReviews = reviews
				.GroupBy(r => r.FlashcardId)
				.Select(g => g.OrderByDescending(r => r.ReviewDate).First())
				.ToList();

			// Filter the latest reviews for those that are due today
			var dueReviews = latestReviews
				.Where(r => r.NextReviewDate.Date == DateTime.Today)
				.ToList();

			// Get distinct flashcards from the due reviews
			var flashcards = dueReviews.Select(r => r.Flashcard).Distinct().ToList();

			var flashcardList = flashcards.ToList();
			if (!flashcardList.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No flashcards found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<FlashcardDto>>(flashcardList);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> CreateFlashcardAsync(CreateFlashcardDto flashcardDto)
	{
		var response = new ApiResponse();

		try
		{
			var flashcard = mapper.Map<Flashcard>(flashcardDto);

			await flashcardRepository.CreateAsync(flashcard);

			var createdFlashcard = await flashcardRepository.GetAsync(filter: f => f.Id == flashcard.Id, includeProperties: "Unit");

			response.IsSucceed = true;
			response.Data = mapper.Map<FlashcardDto>(createdFlashcard);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> UpdateFlashcardAsync(int flashcardId, UpdateFlashcardDto flashcardDto)
	{
		var response = new ApiResponse();

		try
		{
			if (flashcardId != flashcardDto.Id)
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Flashcard ID mismatch"];
				return response;
			}

			var flashcard = await flashcardRepository.GetAsync(filter: f => f.Id == flashcardId, includeProperties: "Unit");

			if (flashcard == null)
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Flashcard not found"];
				return response;
			}

			var updatedFlashcard = mapper.Map(flashcardDto, flashcard);

			await flashcardRepository.UpdateAsync(updatedFlashcard);

			response.IsSucceed = true;
			response.Data = mapper.Map<FlashcardDto>(updatedFlashcard);

			return response;
		}
		catch (Exception ex)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [ex.Message]
			};
		}
	}

	public async Task<ApiResponse> DeleteFlashcardAsync(int id)
	{
		var response = new ApiResponse();

		try
		{
			if (!await flashcardRepository.ExistsAsync(f => f.Id == id))
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Flashcard not found"];
				return response;
			}

			await flashcardRepository.DeleteAsync(id);
			response.IsSucceed = true;

			return response;
		}
		catch (Exception e)
		{
			return new ApiResponse
			{
				IsSucceed = false,
				ErrorMessages = [e.Message]
			};
		}
	}
}