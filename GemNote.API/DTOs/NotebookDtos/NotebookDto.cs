namespace GemNote.API.DTOs.NotebookDtos;

public class NotebookDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public int SectionQty { get; set; }
	public string AppUserId { get; set; }
	public DateTime CreatedAt { get; set; }
}