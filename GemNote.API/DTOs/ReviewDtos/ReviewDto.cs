namespace GemNote.API.DTOs.ReviewDtos;

public class ReviewDto
{
	public int Id { get; set; }

	public int? FlashcardId { get; set; }
	public string? AppUserId { get; set; }

	public DateTime ReviewDate { get; set; }
	public int EaseLevel { get; set; }
	public DateTime NextReviewDate { get; set; }
}