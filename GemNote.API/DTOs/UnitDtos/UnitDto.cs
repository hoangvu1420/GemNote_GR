namespace GemNote.API.DTOs.UnitDtos;

public class UnitDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int CardQty { get; set; }

	public int SectionId { get; set; }
	public string SectionName { get; set; }
}