using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.SectionDtos;

public class CreateSectionDto
{
	[Required]
	public string Name { get; set; }

	public string? Description { get; set; }

	[Required]
	public int NotebookId { get; set; }
}