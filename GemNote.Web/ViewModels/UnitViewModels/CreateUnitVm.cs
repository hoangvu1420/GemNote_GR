using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.UnitViewModels;

public class CreateUnitVm
{
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }
	public int SectionId { get; set; }
}