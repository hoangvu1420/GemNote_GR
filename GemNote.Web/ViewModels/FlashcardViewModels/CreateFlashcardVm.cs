using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.FlashcardViewModels;

public class CreateFlashcardVm
{
	[Required]
	public string Front { get; set; }
	[Required]
	public string Back { get; set; }
	public int UnitId { get; set; }
}