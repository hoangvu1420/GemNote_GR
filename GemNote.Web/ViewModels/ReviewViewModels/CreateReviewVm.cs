using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.ReviewViewModels;

public class CreateReviewVm
{
	[Required]
	public int FlashcardId { get; set; }
	[Required]
	public string AppUserId { get; set; }

	public DateTime ReviewDate { get; set; }
	public int EaseLevel { get; set; }
}