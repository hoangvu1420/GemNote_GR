using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.NotebookViewModels;

public class UpdateNotebookVm
{
	public int Id { get; set; }

	[Required(ErrorMessage = "Name is required")]
	public string Name { get; set; }
	public string? Description { get; set; }

	[Required(ErrorMessage = "Category is required")]
	public string CategoryName { get; set; }

	[Required]
	public string AppUserId { get; set; }
}