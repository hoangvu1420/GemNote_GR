using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.SectionViewModels;

public class UpdateSectionVm
{
	public int Id { get; set; }

	[Required(ErrorMessage = "Name is required")]
	public string Name { get; set; }

	public string? Description { get; set; }
}