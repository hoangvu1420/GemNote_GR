using System.ComponentModel.DataAnnotations;

namespace GemNote.API.Models;

public class Category : BaseEntity
{
	[Required]
	[StringLength(200)]
	public string Name { get; set; }

	[StringLength(2000)]
	public string? Description { get; set; }

	public ICollection<Notebook> Notebooks { get; set; }
}