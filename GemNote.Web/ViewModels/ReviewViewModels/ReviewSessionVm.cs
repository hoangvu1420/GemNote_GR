using GemNote.Web.ViewModels.FlashcardViewModels;

namespace GemNote.Web.ViewModels.ReviewViewModels;

public class ReviewSessionVm
{
	public List<FlashcardVm>? Flashcards { get; set; } = new();
	public int FlashcardCount { get; set; }
	public List<CreateReviewVm> Reviews { get; set; } = new();
	public int ReviewCount { get; set; }
}