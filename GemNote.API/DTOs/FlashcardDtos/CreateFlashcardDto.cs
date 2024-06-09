using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.FlashcardDtos;

public class CreateFlashcardDto
{
	[Required]
	public string Front { get; set; }
	[Required]
	public string Back { get; set; }
	public int UnitId { get; set; }
}