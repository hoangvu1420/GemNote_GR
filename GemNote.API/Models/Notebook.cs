using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemNote.API.Models;

public class Notebook : BaseEntity
{
	[Required]
	[StringLength(200)]
	public string Name { get; set; }

	[StringLength(2000)]
	public string? Description { get; set; }

	public ICollection<Section> Sections { get; set; }

	[ForeignKey(nameof(Category))]
	public int CategoryId { get; set; }
	[Required]
	public Category? Category { get; set; }

	[ForeignKey(nameof(AppUser))]
	public string AppUserId { get; set; }
	[Required]
	public AppUser AppUser { get; set; }
}