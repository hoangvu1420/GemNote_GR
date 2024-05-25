using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GemNote.API.Models;

public class AppUser : IdentityUser
{
	[Required]
	[StringLength(200)]
	public string FirstName { get; set; }

	[Required]
	[StringLength(200)]
	public string LastName { get; set; }

	public string FullName => $"{FirstName} {LastName}";

	[StringLength(2000)]
	public string? AvatarUrl { get; set; }

	[StringLength(200)]
	public string? Language { get; set; }

	public ICollection<Notebook> Notebooks { get; set; }

	public ICollection<CardReviewSession> CardReviewSessions { get; set; }
}