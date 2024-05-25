using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GemNote.API.Models;

public class Section : BaseEntity
{
	[Required]
	[StringLength(200)]
	public string Name { get; set; }

	[StringLength(2000)]
	public string? Description { get; set; }

	public ICollection<Unit> Units { get; set; }

	[ForeignKey(nameof(Notebook))]
	public int NotebookId { get; set; }
	[Required]
	public Notebook Notebook { get; set; }
}