using GemNote.Web.ViewModels.FlashcardViewModels;

namespace GemNote.Web.ViewModels.UnitViewModels;

public class DetailedUnitVm
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int CardQty { get; set; }
	public string SectionName { get; set; }
	public string NotebookName { get; set; }
	public IEnumerable<FlashcardVm> Flashcards { get; set; }
}