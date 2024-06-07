using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.SectionViewModels;

public class CreateSectionVm
{
	[Required(ErrorMessage = "Name is required")]
	public string Name { get; set; }
	public string? Description { get; set; }
	public int NotebookId { get; set; }
}