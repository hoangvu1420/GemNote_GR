using GemNote.Web.ViewModels.FlashcardViewModels;
using GemNote.Web.ViewModels.UnitViewModels;

namespace GemNote.Web.Pages;

public partial class UnitPage
{
	private bool _isLoading = true;
	private string _description = string.Empty;

	private UnitVm _unit = new();
	private IEnumerable<FlashcardVm> _flashcards = new List<FlashcardVm>();

	protected override Task OnInitializedAsync()
	{
		
	}
}