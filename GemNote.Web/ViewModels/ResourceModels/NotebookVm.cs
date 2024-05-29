namespace GemNote.Web.ViewModels.ResourceModels;

public class NotebookVm
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SectionQty { get; set; }
}