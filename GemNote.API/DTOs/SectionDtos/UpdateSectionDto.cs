using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.SectionDtos;

public class UpdateSectionDto
{
	[Required]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	public string? Description { get; set; }
}