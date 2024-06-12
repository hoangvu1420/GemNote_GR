using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemNote.API.Models;

public class CardReviewSession
{
	[Key]
	public int Id { get; set; }

	[ForeignKey(nameof(Flashcard))]
	public int? FlashcardId { get; set; } 
	public Flashcard? Flashcard { get; set; }

	[ForeignKey(nameof(AppUser))]
	public string? AppUserId { get; set; }  
	public AppUser? AppUser { get; set; }

	public DateTime ReviewDate { get; set; }
	public int EaseLevel { get; set; }
	public DateTime NextReviewDate { get; set; }
}