using GemNote.API.DTOs.FlashcardDtos;

namespace GemNote.API.DTOs.UnitDtos;

public class DetailedUnitDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int CardQty { get; set; }
	public string SectionName { get; set; }
	public string NotebookName { get; set; }
	public IEnumerable<FlashcardDto> Flashcards { get; set; }
}