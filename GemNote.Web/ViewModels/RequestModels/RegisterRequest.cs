using System.ComponentModel.DataAnnotations;

namespace GemNote.Web.ViewModels.RequestModels;

public class RegisterRequest
{
	[Required(ErrorMessage = "Email is required")]
	[EmailAddress(ErrorMessage = "Email is not valid")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	[StringLength(16, ErrorMessage = "Password must be at least 8 characters and at most 16 characters", MinimumLength = 8)]
	public string Password { get; set; }

	[Required(ErrorMessage = "First name is required")]
	public string FirstName { get; set; }

	[Required(ErrorMessage = "Last name is required")]
	public string LastName { get; set; }

	public string? Language { get; set; }

	public string? AvatarUrl { get; set; }
}