using GemNote.Web.ViewModels.FlashcardViewModels;

namespace GemNote.Web.ViewModels.UnitViewModels;

public class UnitVm
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int CardQty { get; set; }

	public int SectionId { get; set; }
	public string SectionName { get; set; }
}