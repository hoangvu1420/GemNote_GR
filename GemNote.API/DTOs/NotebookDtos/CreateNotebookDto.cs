using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.NotebookDtos;

public class CreateNotebookDto
{
	[Required]
	public string Name { get; set; }
	public string? Description { get; set; }
	[Required]
	public string CategoryName { get; set; }
	[Required]
	public string AppUserId { get; set; }
}