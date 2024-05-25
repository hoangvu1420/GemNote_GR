using System.ComponentModel.DataAnnotations;

namespace GemNote.API.Models;

public abstract class BaseEntity
{
	[Key]
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
}