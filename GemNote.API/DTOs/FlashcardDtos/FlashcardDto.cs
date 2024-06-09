namespace GemNote.API.DTOs.FlashcardDtos;

public class FlashcardDto
{
	public int Id { get; set; }
	public string Front { get; set; }
	public string Back { get; set; }

	public int Interval { get; set; }
	public int RepetitionCount { get; set; }
	public double EaseFactor { get; set; }

	public int UnitId { get; set; }
	public string UnitName { get; set; }
}