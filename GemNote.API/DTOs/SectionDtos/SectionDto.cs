using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.SectionDtos;

public class SectionDto
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public string NotebookName { get; set; }

	public int UnitQty { get; set; }

	public DateTime CreatedAt { get; set; }
}