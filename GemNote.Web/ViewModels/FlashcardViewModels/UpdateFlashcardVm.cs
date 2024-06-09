using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.FlashcardViewModels;

public class UpdateFlashcardVm
{
	public int Id { get; set; }
	[Required]
	public string Front { get; set; }
	[Required]
	public string Back { get; set; }
}