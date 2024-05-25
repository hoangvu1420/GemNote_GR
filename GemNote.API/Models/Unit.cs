using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemNote.API.Models;

public class Unit : BaseEntity
{
	[Required]
	[StringLength(200)]
	public string Name { get; set; }

	[StringLength(2000)]
	public string? Description { get; set; }

	[ForeignKey(nameof(Section))]
	public int SectionId { get; set; }
	[Required]
	public Section Section { get; set; } // navigation property for one-to-many relationship with Section

	public ICollection<Flashcard> Flashcards { get; set; }
}