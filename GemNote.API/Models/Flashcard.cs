using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemNote.API.Models;

public class Flashcard : BaseEntity
{
	[Required]
	[StringLength(10000)]
	public string Front { get; set; }

	[Required]
	[StringLength(10000)]
	public string Back { get; set; }

	public int Interval { get; set; } = 1;
	public int RepetitionCount { get; set; } = 0;
	public double EaseFactor { get; set; } = 2.5;

	public ICollection<CardReviewSession> CardReviewSessions { get; set; }

	[ForeignKey(nameof(Unit))]
	public int UnitId { get; set; }
	[Required]
	public Unit Unit { get; set; }
}
