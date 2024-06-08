using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.UnitDtos;

public class CreateUnitDto
{
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }
	public int SectionId { get; set; }
}