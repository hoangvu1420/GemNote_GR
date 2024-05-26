using System.ComponentModel.DataAnnotations;

namespace GemNote.API.DTOs.AuthDtos;

public class RegisterRequestDto
{
	[Required(ErrorMessage = "First name is required")]
	public string FirstName { get; init; }

	[Required(ErrorMessage = "Last name is required")]
	public string LastName { get; init; }

	public string? Language { get; init; }

	public string? AvatarUrl { get; init; }

	[Required(ErrorMessage = "Email is required")]
	[EmailAddress(ErrorMessage = "Invalid email address")]
	public string Email { get; init; }

	[Required(ErrorMessage = "Password is required")]
	public string Password { get; init; }
}