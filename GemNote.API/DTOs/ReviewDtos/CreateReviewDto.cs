namespace GemNote.API.DTOs.ReviewDtos;

public class CreateReviewDto
{
	public int? FlashcardId { get; set; }
	public string? AppUserId { get; set; }

	public DateTime ReviewDate { get; set; }
	public int EaseLevel { get; set; }
}