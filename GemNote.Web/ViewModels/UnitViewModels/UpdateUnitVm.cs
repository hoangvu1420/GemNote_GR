using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.UnitViewModels;

public class UpdateUnitVm
{
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }
}