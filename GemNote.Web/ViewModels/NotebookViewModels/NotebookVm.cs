namespace GemNote.Web.ViewModels.NotebookViewModels;

public class NotebookVm
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public string Category { get; set; }
	public int SectionQty { get; set; }
	public string AppUserId { get; set; }
	public DateTime CreatedAt { get; set; }
}