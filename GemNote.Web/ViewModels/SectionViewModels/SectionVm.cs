namespace GemNote.Web.ViewModels.SectionViewModels;

public class SectionVm
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public int UnitQty { get; set; }
	public int NotebookId { get; set; }
	public DateTime CreatedAt { get; set; }
}