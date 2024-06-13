using AutoMapper;
using GemNote.API.DTOs;
using GemNote.API.DTOs.ReviewDtos;
using GemNote.API.Models;
using GemNote.API.Repositories.Contracts;
using GemNote.API.Services.Contracts;

namespace GemNote.API.Services.Implementations;

public class ReviewService(
	IReviewRepository reviewRepository,
	IFlashcardRepository flashcardRepository,
	IMapper mapper) : IReviewService
{
	public async Task<ApiResponse> CreateReviewAsync(CreateReviewDto reviewDto)
	{
		var response = new ApiResponse();

		try
		{
			var review = mapper.Map<CardReviewSession>(reviewDto);
			await SpacedRepetitionReview(review);

			await reviewRepository.CreateAsync(review);

			response.IsSucceed = true;

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

	public async Task<ApiResponse> CreateRangeReviewAsync(IEnumerable<CreateReviewDto> reviewDtos)
	{
		var response = new ApiResponse();

		try
		{
			var reviews = mapper.Map<IEnumerable<CardReviewSession>>(reviewDtos);

			var cardReviewSessions = reviews.ToList();
			foreach (var review in cardReviewSessions)
			{
				await SpacedRepetitionReview(review);
			}
			await reviewRepository.CreateRangeAsync(cardReviewSessions);

			response.IsSucceed = true;
			response.Data = cardReviewSessions.Select(r => r.Id);

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

	public async Task<ApiResponse> DeleteReviewAsync(int reviewId)
	{
		var response = new ApiResponse();

		try
		{
			var review = await reviewRepository.GetAsync(filter: r => r.Id == reviewId);
			if (review == null)
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Review not found"];
				return response;
			}

			await reviewRepository.DeleteAsync(reviewId);

			response.IsSucceed = true;

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

	public async Task<ApiResponse> DeleteRangeReviewAsync(IEnumerable<int> reviewIds)
	{
		var response = new ApiResponse();

		try
		{
			var reviews = await reviewRepository.GetAllAsync(filter: r => reviewIds.Contains(r.Id));
			var cardReviewSessions = reviews.ToList();
			if (!cardReviewSessions.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["Reviews not found"];
				return response;
			}

			await reviewRepository.DeleteRangeAsync(cardReviewSessions);

			response.IsSucceed = true;

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

	public async Task<ApiResponse> GetDueReviewsByUserId(string userId)
	{
		var response = new ApiResponse();

		try
		{
			var reviews = await reviewRepository.GetAllAsync(filter: r =>
				r.AppUserId == userId && r.NextReviewDate.Date == DateTime.Today);

			var cardReviewSessions = reviews.ToList();
			if (!cardReviewSessions.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No reviews found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<ReviewDto>>(cardReviewSessions);

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

	public async Task<ApiResponse> GetReviewsByUserId(string userId)
	{
		var response = new ApiResponse();

		try
		{
			var reviews = await reviewRepository.GetAllAsync(filter: r => r.AppUserId == userId);

			var cardReviewSessions = reviews.ToList();
			if (!cardReviewSessions.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No reviews found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<ReviewDto>>(cardReviewSessions);

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

	public async Task<ApiResponse> GetReviewsByFlashcardId(int flashcardId)
	{
		var response = new ApiResponse();

		try
		{
			var reviews = await reviewRepository.GetAllAsync(filter: r => r.FlashcardId == flashcardId);

			var cardReviewSessions = reviews.ToList();
			if (!cardReviewSessions.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No reviews found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<ReviewDto>>(cardReviewSessions);

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

	public async Task<ApiResponse> GetReviewsByUnitId(int unitId)
	{
		var response = new ApiResponse();

		try
		{
			var flashcards = await flashcardRepository.GetAllAsync(filter: f => f.UnitId == unitId, includeProperties: "CardReviewSessions");

			var reviews = flashcards.SelectMany(f => f.CardReviewSessions);

			var cardReviewSessions = reviews.ToList();
			if (!cardReviewSessions.Any())
			{
				response.IsSucceed = false;
				response.ErrorMessages = ["No reviews found"];
				return response;
			}

			response.IsSucceed = true;
			response.Data = mapper.Map<IEnumerable<ReviewDto>>(cardReviewSessions);

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

	private async Task SpacedRepetitionReview(CardReviewSession review)
	{
		var flashcard = await flashcardRepository.GetAsync(filter: f => f.Id == review.FlashcardId);

		if (flashcard == null)
			throw new Exception("Flashcard not found");
		
		var easeLevel = review.EaseLevel;
		var easeFactor = flashcard.EaseFactor + (0.1 - (5 - easeLevel) * (0.08 + (5 - easeLevel) * 0.02));
		easeFactor = Math.Max(1.3, easeFactor);
		flashcard.EaseFactor = easeFactor;

		if (easeLevel < 3)
		{
			flashcard.Interval = 1;
			flashcard.RepetitionCount = 0;
		}
		else
		{
			// Calculate new interval
			if (flashcard.RepetitionCount == 0)
			{
				flashcard.Interval = 1;
			}
			else if (flashcard.RepetitionCount == 1)
			{
				flashcard.Interval = 6;
			}
			else
			{
				flashcard.Interval = (int)(flashcard.Interval * flashcard.EaseFactor);
			}

			// Increment repetition count
			flashcard.RepetitionCount += 1;
		}

		var nextReviewDate = DateTime.UtcNow.AddDays(flashcard.Interval);

		await flashcardRepository.UpdateAsync(flashcard);

		review.NextReviewDate = nextReviewDate;
	}
}